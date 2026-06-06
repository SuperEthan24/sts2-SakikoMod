using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SakikoMod.SakikoModCode.Powers;

public class GoldNextTurnPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;

    public override async Task BeforeSideTurnStart(PlayerChoiceContext ctx, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Side) return;
        int amt = Amount;
        if (base.Owner.Player != null)
        {
            if (amt != 0)
            {
                await PlayerCmd.GainGold(amt, Owner.Player);
            }
        }
        await PowerCmd.Remove<GoldNextTurnPower>(Owner);
    }
}