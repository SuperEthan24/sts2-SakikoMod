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
		new DamageVar(8, ValueProp.Move),
		new DynamicVar("AttackTimes", 4),
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
		DynamicVars.Damage.UpgradeValueBy(3);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		var cs = Owner.Creature.CombatState;
		int num = DynamicVars["AttackTimes"].IntValue;
		if (cs != null && cs.HittableEnemies.Count > 0)
		{
			if (SakikoModCmd.IsDecrescent(base.Owner.Creature))
			{
				List<CardModel> list = PileType.Hand.GetPile(base.Owner).Cards.ToList();
				int cardCount = list.Count;
				foreach (CardModel item in list)
				{
					await SakikoModCmd.InGameDelete(base.Owner.Creature, ctx, item);
				}

				num += cardCount;
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(num).FromCard(this)
				.TargetingRandomOpponents(cs).Execute(ctx);
		}
	}

	public ReverberationOfDecrescent() : base(3, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy) {}
}