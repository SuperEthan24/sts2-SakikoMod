using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace SakikoMod.SakikoModCode.Powers;

public class SakikoLightPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new EnergyVar(0),
        new DynamicVar("MaximumStack", 30)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    public override Task AfterPowerAmountChanged(PlayerChoiceContext ctx, PowerModel power, decimal amount,
        Creature? applier, CardModel? cardSource)
    {
        if (power.Owner != base.Owner) return Task.CompletedTask;
        if (base.Owner.HasPower<HaruhikagePower>())
        {
            DynamicVars["MaximumStack"].BaseValue = 60;
        }
        if (power == this && Amount > DynamicVars["MaximumStack"].BaseValue)
        {
            SetAmount((int)DynamicVars["MaximumStack"].BaseValue, true);
        }

        DynamicVars.Energy.BaseValue = Amount / 10;
        return Task.CompletedTask;
    }
    
    public override async Task AfterEnergyReset(Player player)
    {
        if (player == base.Owner.Player)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, player);
        }
    }
}