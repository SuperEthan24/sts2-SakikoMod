using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace SakikoMod.SakikoModCode.Powers;

public class SymbolIiAirPower : CustomPowerModel
{
	public override PowerStackType StackType => PowerStackType.Counter;
	public override PowerType Type => PowerType.Debuff;

	public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
	{
		if (autoPlayType == AutoPlayType.SlyDiscard) return true;
		if (card.Owner != base.Owner.Player) return true;
		if (AmountOnTurnStart > 0)
		{
			if (card.Type == CardType.Attack) return false;
		}
		return true;
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> _)
	{
		if (side != base.Owner.Side) return;
		if (AmountOnTurnStart > 0)
			await PowerCmd.Decrement(this);
	}
}