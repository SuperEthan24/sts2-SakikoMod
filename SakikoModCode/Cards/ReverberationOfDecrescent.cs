using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class ReverberationOfDecrescent : SakikoCharacterBaseCard
{
	protected override bool ShouldGlowGoldInternal => SakikoModCmd.IsDecrescent(base.Owner.Creature);

	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(7, ValueProp.Move),
		new DynamicVar("AttackTimes", 4),
		new DamageVar("ExtraHit", 11, ValueProp.Move)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<DecrescentPower>();
			yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Deletion);
		}
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(4);
		DynamicVars["ExtraHit"].UpgradeValueBy(3);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		var cs = Owner.Creature.CombatState;
		int num = (int)DynamicVars["AttackTimes"].BaseValue;
		if (cs != null && cs.HittableEnemies.Count > 0 && num > 0)
		{
			await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
				.FromCard(this).TargetingRandomOpponents(cs, allowDuplicates: true)
				.WithHitCount(num).Execute(ctx);
		}
		if (!SakikoModCmd.IsDecrescent(base.Owner.Creature)) return;
		foreach (CardModel card in PileType.Hand.GetPile(base.Owner).Cards.ToList())
		{
			if (cs != null && cs.HittableEnemies.Count > 0)
			{
				await SakikoModCmd.InGameDelete(base.Owner.Creature, ctx, card);
				await DamageCmd.Attack(DynamicVars["ExtraHit"].BaseValue)
					.FromCard(this).TargetingRandomOpponents(cs, allowDuplicates: true)
					.Execute(ctx);
			}
			else break;
		}
	}

	public ReverberationOfDecrescent() : base(3, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy) {}
}