using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SakikoMod.SakikoModCode.Powers;

public class HaruhikagePower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;

	public override async Task BeforeSideTurnStart(PlayerChoiceContext ctx, CombatSide side, IReadOnlyList<Creature> participants,
		ICombatState combatState)
	{
		if (side != base.Owner.Side) return;
		if (base.Owner.Player != null && !CombatManager.Instance.PlayersTakingExtraTurn.Contains(base.Owner.Player))
		{
			await PowerCmd.Apply<ExtraTurnPower>(ctx, base.Owner, Amount, base.Owner, null);
		}
	}
}