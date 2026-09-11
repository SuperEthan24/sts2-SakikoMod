using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class LingeringSound : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(7, ValueProp.Move)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(2);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (play.Target != null)
		{
			AttackCommand command = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
				.FromCard(this, play).Targeting(play.Target).Execute(ctx);
			if (play.Target is { CombatId: not null })
			{
				LingeringPower? lingering = await PowerCmd.Apply<LingeringPower>(ctx, base.Owner.Creature,
					command.Results.First().First().TotalDamage, base.Owner.Creature, this);
				if (lingering != null)
				{
					lingering.DynamicVars["Enemy"].BaseValue = (decimal)play.Target.CombatId;
				}
			}
		}
	}

	public LingeringSound() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {}
}