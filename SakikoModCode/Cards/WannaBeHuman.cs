using System.ComponentModel;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class WannaBeHuman : SakikoCharacterBaseCard
{
    private readonly List<DynamicVar> _vars = new()
    {
        new CalculationBaseVar(10m),
        new CalculationExtraVar(6m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => card.Owner.PlayerCombatState.AllCards.Count((CardModel c) => c.Type is CardType.Curse && c != card && c.Pile?.Type is PileType.Exhaust))
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(6);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, DynamicVars.CalculatedBlock.BaseValue,
            DynamicVars.CalculatedBlock.Props, play);
    }

    public WannaBeHuman() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {}
}