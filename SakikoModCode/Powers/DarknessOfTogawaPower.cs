using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace SakikoMod.SakikoModCode.Powers;

public class DarknessOfTogawaPower : CustomPowerModel
{
	public override PowerStackType StackType => PowerStackType.Counter;
	public override PowerType Type => PowerType.Buff;

	public override bool ShouldClearBlock(Creature creature)
	{
		if (creature == base.Owner && base.Owner.Player != null)
		{
			Flash();
			PlayerCmd.GainGold(Amount * base.Owner.Block, base.Owner.Player);
		}
		return true;
	}
}