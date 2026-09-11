using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class STheWay : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new CalculationBaseVar(8m),
		new ExtraDamageVar(1m),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, Creature? _)
		{
			IEnumerable<LingeringPower>? lingeringPowers = card.Owner.Creature.GetPowerInstances<LingeringPower>();
			return lingeringPowers.Sum(lingeringPower => lingeringPower.Amount);
		})
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars.CalculationBase.UpgradeValueBy(4);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		var cs = Owner.Creature.CombatState;
		if (cs is { HittableEnemies.Count: > 0 })
		{
			await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this, play).TargetingAllOpponents(cs)
				.Execute(ctx);
			List<LingeringPower> lingeringPowers = base.Owner.Creature.GetPowerInstances<LingeringPower>().ToList();
			foreach (var lingeringPower in lingeringPowers)
			{
				await PowerCmd.Remove(lingeringPower);
			}
		}
	}

	public STheWay() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) {}
}