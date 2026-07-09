using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace SakikoMod.SakikoModCode.Powers;

public class ExtraTurnPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
	public override bool AllowNegative => false;

	public override bool ShouldTakeExtraTurn(Player player)
	{
		return (Amount > 0) && (player == base.Owner.Player);
	}

	public override async Task AfterTakingExtraTurn(Player player)
	{
		if (player == base.Owner.Player)
		{
			await PowerCmd.Decrement(this);
		}
	}
}