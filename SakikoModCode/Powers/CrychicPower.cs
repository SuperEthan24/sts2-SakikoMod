using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SakikoMod.SakikoModCode.Powers;

public class CrychicPower : CustomPowerModel
{
	public override PowerStackType StackType => PowerStackType.Counter;
	public override PowerType Type => PowerType.Buff;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<VigorPower>();
		}
	}

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
	{
		if (player != base.Owner.Player) return;
		IEnumerable<CardModel> cards = await CardSelectCmd.FromHandForDiscard(ctx, player,
			new CardSelectorPrefs(base.SelectionScreenPrompt, 0, 999999),
			model => model.Type == CardType.Curse, this);
		if (cards.Count() != 0)
		{
			await CardCmd.Discard(ctx, cards);
		}

		await PowerCmd.Apply<VigorPower>(ctx, base.Owner, (10 - cards.Count()) * Amount, base.Owner, null);
	}
}