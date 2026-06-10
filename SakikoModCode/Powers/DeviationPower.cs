using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace SakikoMod.SakikoModCode.Powers;

public class DeviationPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new DynamicVar("MaxStack", 10)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext ctx, PowerModel power, decimal amount,
        Creature? applier, CardModel? cardSource)
    {
        if (power.Owner != base.Owner) return;
        if (base.Owner.HasPower<EntanglementPower>()) DynamicVars["MaxStack"].BaseValue = 20;
        else DynamicVars["MaxStack"].BaseValue = 10;
        if (power == this && Amount >= DynamicVars["MaxStack"].BaseValue)
        {
            SetAmount((int)(Amount - DynamicVars["MaxStack"].BaseValue));
            PlayerCmd.EndTurn(base.Owner.Player, false);
            await PowerCmd.Apply<EntanglementPower>(ctx, base.Owner, base.Owner.HasPower<EntanglementPower>() ? 1 : 2, base.Owner, null); 
        }
    }
}