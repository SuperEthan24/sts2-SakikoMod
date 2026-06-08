using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Powers;

public class CardAddPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override bool AllowNegative => false;
    protected override bool IsVisibleInternal => false;
    
    [SavedProperty] private HashSet<CardModel> _cards = new HashSet<CardModel>();
    
    public void DeleteCard(CardModel card)
    {
        if (_cards.Contains(card))
            _cards.Remove(card);
    }
    
    public void AddCard(CardModel card)
    {
        _cards.Add(card);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card.Keywords.Contains(SakikoModKeywords.Addition))
        {
            await SakikoModCmd.InGameAdd(base.Owner, ctx, play.Card, PileType.None);
        }
    }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        foreach (CardModel c in _cards)
        {
            CardModel card = base.Owner.Player.RunState.CreateCard(c.CanonicalInstance, base.Owner.Player);
            await CardPileCmd.Add(card, PileType.Deck);
        }
        _cards.Clear();
    }
}