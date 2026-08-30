using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace SakikoMod.SakikoModCode.Powers;

public class AveMujicaPower : CustomPowerModel
{
	public override PowerStackType StackType => PowerStackType.Counter;
	public override PowerType Type => PowerType.Buff;
	private bool _used = false;

	public override async Task AfterCardExhausted(PlayerChoiceContext _, CardModel card, bool causedByEthereal)
	{
		if (card.Owner != base.Owner.Player) return;
		if (causedByEthereal) return;
		if (_used) return;
		for (int i = 0; i < Amount; i++)
		{
			CardModel c = card.CreateClone();
			CardCmd.ApplyKeyword(c, CardKeyword.Ethereal);
			await CardPileCmd.AddGeneratedCardToCombat(c, PileType.Hand, base.Owner.Player);
			Flash();
		}
		_used = true;
	}

	public override Task AfterSideTurnEnd(PlayerChoiceContext _, CombatSide side, IEnumerable<Creature> __)
	{
		if (side != base.Owner.Side) return Task.CompletedTask;
		_used = false;
		return Task.CompletedTask;
	}
}