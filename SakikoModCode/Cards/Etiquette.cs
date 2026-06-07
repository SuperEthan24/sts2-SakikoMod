using BaseLib.Utils;
using Godot.Collections;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class Etiquette : SakikoModBaseCard
{
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        CardModel c = base.CombatState.CreateCard<Desuwa>(base.Owner);
        await SakikoModCmd.InGameAdd(base.Owner.Creature, ctx, c);
        await CardPileCmd.Add(c, PileType.Draw, CardPilePosition.Random);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(c, PileType.Draw, CardPilePosition.Random), 2f);
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromCard<Desuwa>();
            yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Addition);
        }
    }

    public Etiquette() : base(2, CardType.Power, CardRarity.Rare, TargetType.None) { }
}