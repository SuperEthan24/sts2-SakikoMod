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
    private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Ethereal };
    public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;
    
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        CardModel c = base.CombatState.CreateCard<Desuwa>(base.Owner);
        await SakikoModCmd.InGameAdd(base.Owner.Creature, ctx, c, PileType.Draw);
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromCard<Desuwa>();
            yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Addition);
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }

    public Etiquette() : base(2, CardType.Power, CardRarity.Rare, TargetType.None) { }
}