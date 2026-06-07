using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using SakikoMod.SakikoModCode.Cards;

namespace SakikoMod.SakikoModCode;

public class RibbonReward : CustomReward
{
    private readonly List<CardModel> _ribbonCards = new List<CardModel>();
    private readonly List<CardCreationResult> _cards = new List<CardCreationResult>();
    private NCardRewardSelectionScreen? _currentlyShownScreen;
    private readonly PlayerChoiceSynchronizer _synchronizer = RunManager.Instance.PlayerChoiceSynchronizer;

    protected override RewardType RewardType => RewardType.None;
    public override bool IsPopulated => true;
    public override LocString Description => GetLoc();
    public override void Populate() { }
    public override void MarkContentAsSeen() { }
    protected override string? IconPath => ImageHelper.GetImagePath("ui/reward_screen/reward_icon_card.png");

    protected override async Task<bool> OnSelect()
    {
        bool rewardComplete = false;
        bool endSelection = false;
        List<CardModel> chosenCardIds = new List<CardModel>();
        CardCreationOptions options = new CardCreationOptions(
            _ribbonCards, CardCreationSource.None,
            CardRarityOddsType.Uniform).WithFlags(CardCreationFlags.NoModifications);
        CardReward reward = new CardReward(_ribbonCards, CardCreationSource.Other, base.Player, options);
        IReadOnlyList<CardRewardAlternative> cardRewardOption = CardRewardAlternative.Generate(reward);
        if (LocalContext.IsMe(base.Player))
        {
            _currentlyShownScreen = NCardRewardSelectionScreen.ShowScreen(_cards, cardRewardOption);
        }

        while (!endSelection)
        {
            uint choiceId = _synchronizer.ReserveChoiceId(base.Player);
            int? num;
            CardModel obtainedCard;
            if (LocalContext.IsMe(base.Player))
            {
                if (_currentlyShownScreen != null)
                {
                    num = await _currentlyShownScreen.OptionSelected();
                }
                else
                {
                    CardRewardSelection? selection = CardSelectCmd.Selector?.GetSelectedCardReward(_cards, cardRewardOption);
                    if (!selection.HasValue)
                    {
                        throw new InvalidOperationException("Card selector unset during test!");
                    }
                    if (selection.Value.alternative != null)
                    {
                        obtainedCard = null;
                        num = _cards.Count + cardRewardOption.FirstIndex((CardRewardAlternative r) => r == selection.Value.alternative);
                    }
                    else
                    {
                        obtainedCard = selection.Value.card;
                        num = ((obtainedCard != null) ? new int?(_cards.FirstIndex((CardCreationResult c) => c.Card == selection.Value.card)) : ((int?)null));
                    }
                }
                PlayerChoiceResult result = PlayerChoiceResult.FromIndex(num);
                _synchronizer.SyncLocalChoice(base.Player, choiceId, result);
            }
            else
            {
                num = (await _synchronizer.WaitForRemoteChoice(base.Player, choiceId)).AsIndexOrNull();
            }
            NCardHolder cardHolder;
            CardRewardAlternative cardRewardAlternative;
            if (num.HasValue)
            {
                if (num < _cards.Count)
                {
                    obtainedCard = _cards[num.Value].Card;
                    rewardComplete = true;
                    endSelection = !Hook.ShouldAllowSelectingMoreCardRewards(base.Player.RunState, base.Player, reward);
                    cardHolder = _currentlyShownScreen?.GetCardHolder(obtainedCard);
                    cardRewardAlternative = null;
                }
                else
                {
                    if (!(num < _cards.Count + cardRewardOption.Count))
                    {
                        Log.Error($"Received bad player choice index {num} for a card reward with {_cards.Count} cards and {cardRewardOption.Count} alternatives!");
                        continue;
                    }
                    cardRewardAlternative = cardRewardOption[num.Value - _cards.Count];
                    rewardComplete = cardRewardAlternative.AfterSelected == PostAlternateCardRewardAction.EndSelectionAndCompleteReward;
                    PostAlternateCardRewardAction afterSelected = cardRewardAlternative.AfterSelected;
                    bool flag = (uint)(afterSelected - 1) <= 1u;
                    endSelection = flag;
                    cardHolder = null;
                    obtainedCard = null;
                }
            }
            else
            {
                rewardComplete = false;
                endSelection = true;
                cardHolder = null;
                obtainedCard = null;
                cardRewardAlternative = null;
            }
            if (!(obtainedCard != null || cardRewardAlternative != null || rewardComplete))
            {
                continue;
            }
            if (obtainedCard != null)
            {
                CardPileAddResult cardPileAddResult = await CardPileCmd.Add(obtainedCard, PileType.Deck);
                if (cardPileAddResult.success)
                {
                    obtainedCard = cardPileAddResult.cardAdded;
                    chosenCardIds.Add(obtainedCard);
                    _cards.RemoveAll((CardCreationResult c) => c.Card == obtainedCard);
                    if (cardHolder != null)
                    {
                        NCard cardNode = cardHolder.CardNode;
                        NRun.Instance.GlobalUi.ReparentCard(cardNode);
                        cardHolder.QueueFreeSafely();
                        NRun.Instance.GlobalUi.TopBar.TrailContainer.AddChildSafely(NCardFlyVfx.Create(cardNode, PileType.Deck, isAddingToPile: true, obtainedCard.Owner.Character.TrailPath));
                    }
                    Log.Info($"Player {base.Player.NetId} obtained {obtainedCard.Id} from card reward");
                }
            }
            else if (cardRewardAlternative != null)
            {
                await cardRewardAlternative.OnSelect();
            }
        }
        foreach (CardModel item in chosenCardIds)
        {
            base.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(base.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(item, wasPicked: true));
        }
        if (rewardComplete)
        {
            foreach (CardCreationResult card in _cards)
            {
                base.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(base.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(card.Card, wasPicked: false));
            }
        }
        if (_currentlyShownScreen != null)
        {
            NOverlayStack.Instance?.Remove(_currentlyShownScreen);
            _currentlyShownScreen = null;
        }
        return rewardComplete;
    }

    public static CustomReward CreateFromSerializable(SerializableReward save, Player player)
    {
        return new RibbonReward(player);
    }
    public override CreateRewardFromSave<CustomReward> DeserializeMethod => CreateFromSerializable;

    public RibbonReward(Player player) : base(player)
    {
        _ribbonCards.Add(player.RunState.CreateCard<Nihil>(player));
        _ribbonCards.Add(player.RunState.CreateCard<Desire>(player));
        _cards = _ribbonCards.Select((CardModel c) => new CardCreationResult(c)).ToList();
    }
}