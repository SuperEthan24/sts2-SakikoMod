using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace SakikoMod.SakikoModCode.Powers;

public class LingeringPower : CustomPowerModel
{
	public override PowerStackType StackType => PowerStackType.Counter;
	public override PowerType Type => PowerType.Buff;
	public override bool AllowNegative => false;
	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
	
	private readonly List<DynamicVar> _vars = new()
	{
		new DynamicVar("Enemy", 1)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	public override async Task BeforeSideTurnStart(PlayerChoiceContext ctx, CombatSide side, IReadOnlyList<Creature> _,
		ICombatState combatState)
	{
		if (side != base.Owner.Side) return;
		await CreatureCmd.Damage(ctx, combatState.Enemies.First(c => c.CombatId == DynamicVars["Enemy"].IntValue),
			Amount, ValueProp.Unpowered, null, null);
		await PowerCmd.Remove(this);
	}
}