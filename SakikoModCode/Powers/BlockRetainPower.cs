using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace SakikoMod.SakikoModCode.Powers;

public class BlockRetainPower : CustomPowerModel
{
	public override PowerStackType StackType => PowerStackType.Counter;
	public override PowerType Type => PowerType.Buff;

	public override bool ShouldClearBlock(Creature _)
	{
		Flash();
		return false;
	}

	public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> _, ICombatState __)
	{
		if (side != base.Owner.Side) return;
		await PowerCmd.Decrement(this);
	}
}