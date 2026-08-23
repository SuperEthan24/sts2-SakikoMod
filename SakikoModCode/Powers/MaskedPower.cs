using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace SakikoMod.SakikoModCode.Powers;

public class MaskedPower : CustomPowerModel
{
	public override PowerStackType StackType => PowerStackType.Counter;
	public override PowerType Type => PowerType.Buff;

	public override async Task AfterDamageReceived(PlayerChoiceContext ctx, Creature target, DamageResult result, ValueProp props,
		Creature? dealer, CardModel? cardSource)
	{
		if (target != base.Owner) return;
		if (result.WasBlockBroken) return;
		Flash();
		await PowerCmd.Apply<BlockNextTurnPower>(ctx, base.Owner, Amount, base.Owner, null);
	}
}