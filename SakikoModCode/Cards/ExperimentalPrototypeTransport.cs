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
			attackTimes = 4;
			await CreatureCmd.LoseBlock(ctx, base.Owner.Creature, base.Owner.Creature.Block / 2, base.Owner.Creature);
		}
		await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
			.FromCard(this, play).WithHitCount(attackTimes).Targeting(play.Target).Execute(ctx);
	}

	public ExperimentalPrototypeTransport() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {}
}