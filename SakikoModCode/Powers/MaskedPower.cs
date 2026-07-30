using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
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
	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
	
	private bool _playedSkillCard = false;

	public override Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay play)
	{
		if (play.Player != base.Owner.Player) return Task.CompletedTask;
		if (play.Card.Type == CardType.Skill)
		{
			_playedSkillCard = true;
		}
		return Task.CompletedTask;
	}

	public override Task AfterSideTurnEnd(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side != base.Owner.Side)
		{
			return Task.CompletedTask;
		}
		_playedSkillCard = false;
		return Task.CompletedTask;
	}

	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay) 
	{
		if (target != base.Owner)
		{
			return 1m;
		}
		if (!props.IsPoweredAttack())
		{
			return 1m;
		}
		if (dealer == null)
		{
			return 1m;
		}
		if (_playedSkillCard)
		{
			return 1m;
		}
		return 1m - 0.01m * Amount;
	}
}