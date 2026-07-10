using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class AwakeningOfCrescent : SakikoCharacterBaseCard
{
	protected override bool ShouldGlowGoldInternal =>
		CombatManager.Instance.PlayersTakingExtraTurn.Contains(base.Owner);

	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(8, ValueProp.Move),
		new RepeatVar(2)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars.Repeat.UpgradeValueBy(1);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (play.Target == null) return;
		int attackTimes = 1;
		if (CombatManager.Instance.PlayersTakingExtraTurn.Contains(base.Owner))
		{
			attackTimes += DynamicVars.Repeat.IntValue;
		}
		await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
			.FromCard(this, play).WithHitCount(attackTimes).Targeting(play.Target).Execute(ctx);
	}

	public AwakeningOfCrescent() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) {}
}