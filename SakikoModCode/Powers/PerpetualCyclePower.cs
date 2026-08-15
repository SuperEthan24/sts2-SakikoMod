using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace SakikoMod.SakikoModCode.Powers;

public class PerpetualCyclePower : CustomPowerModel {
	private class Data
	{
		public int DamageCounter = 0;
	}

	protected override object? InitInternalData()
	{
		return new Data();
	}

	public override int DisplayAmount => Math.Min(GetInternalData<Data>().DamageCounter, 10);

	public override bool ShouldClearBlock(Creature creature)
	{
		Data data = GetInternalData<Data>();
		bool result = false;
		if (data.DamageCounter > 10)
		{
			Flash();
			result = true;
		}
		data.DamageCounter = 0;
		InvokeDisplayAmountChanged();
		return result;
	}

	public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
		Creature? dealer, CardModel? cardSource)
	{
		Data data = GetInternalData<Data>();
		if (target != base.Owner) return;
		data.DamageCounter += result.UnblockedDamage;
		if (data.DamageCounter > 10)
		{
			Flash();
		}
		InvokeDisplayAmountChanged();
		await Cmd.Wait(0.01f);
	}

	public override PowerStackType StackType => PowerStackType.Counter;
	public override PowerType Type => PowerType.Buff;
	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
}