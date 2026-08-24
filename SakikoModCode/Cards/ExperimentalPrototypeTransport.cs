using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class ExperimentalPrototypeTransport : SakikoCharacterBaseCard
{
	protected override bool ShouldGlowGoldInternal => base.Owner.Creature.Block >= DynamicVars.Block.IntValue;

	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(10, ValueProp.Move),
		new BlockVar(10, ValueProp.Unpowered),
		new DamageVar("SelfDamage", 8, ValueProp.Unpowered),
		new RepeatVar(3)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(5);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (play.Target == null) return;
		int attackTimes = 1;
		if (base.Owner.Creature.Block >= DynamicVars.Block.IntValue)
		{
			attackTimes += DynamicVars.Repeat.IntValue;
			await DamageCmd.Attack(DynamicVars["SelfDamage"].BaseValue)
				.FromCard(this, play).Targeting(base.Owner.Creature).Execute(ctx);
		}
		await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
			.FromCard(this, play).WithHitCount(attackTimes).Targeting(play.Target).Execute(ctx);
	}

	public ExperimentalPrototypeTransport() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {}
}