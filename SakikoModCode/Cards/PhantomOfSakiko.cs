using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class PhantomOfSakiko : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new CalculationBaseVar(8m),
		new ExtraDamageVar(5m),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => card.Owner.PlayerCombatState.AllCards.Count((CardModel c) => c.Type is CardType.Curse && c != card))
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	protected override void OnUpgrade()
	{
		DynamicVars.CalculationBase.UpgradeValueBy(4);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (play.Target != null)
		{
			await DamageCmd.Attack(DynamicVars.CalculatedDamage)
				.FromCard(this).Targeting(play.Target).Execute(ctx);
		}
	}

	public PhantomOfSakiko() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) {}
}