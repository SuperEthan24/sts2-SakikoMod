using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class MasqueradeOfPlenilune : SakikoCharacterBaseCard
{
	protected override bool ShouldGlowGoldInternal => SakikoModCmd.IsPlenilune(base.Owner.Creature);
	
	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar("MainHit", 9, ValueProp.Move),
		new DamageVar("ExtraHit", 5, ValueProp.Move),
		new DynamicVar("ExtraAttack", 3)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<PlenilunePower>();
		}
	}

	protected override void OnUpgrade()
	{
		DynamicVars["MainHit"].UpgradeValueBy(3);
		DynamicVars["ExtraHit"].UpgradeValueBy(2);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		var cs = Owner.Creature.CombatState;
		if (cs != null && cs.HittableEnemies.Count > 0)
		{
			await DamageCmd.Attack(DynamicVars["MainHit"].BaseValue).FromCard(this).TargetingAllOpponents(cs)
				.Execute(ctx);
		}
		int num = 1 + (SakikoModCmd.IsPlenilune(base.Owner.Creature) ? (int)DynamicVars["ExtraAttack"].BaseValue : 0);
		if (play.Target != null)
		{
			await DamageCmd.Attack(DynamicVars["ExtraHit"].BaseValue)
				.FromCard(this).WithHitCount(num).Targeting(play.Target).Execute(ctx);
		}
	}

	public MasqueradeOfPlenilune() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {}
}