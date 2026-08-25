using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Sophie : SakikoCharacterBaseCard
{
	protected override bool IsPlayable => base.Owner.Gold >= DynamicVars["GoldCost"].BaseValue;
	protected override bool ShouldGlowRedInternal => !IsPlayable;

	private readonly List<DynamicVar> _vars = new()
	{
		new PowerVar<SophiePower>(3),
		new GoldCostVar(15)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<SophiePower>();
			yield return HoverTipFactory.FromPower<StrengthPower>();
		}
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Power<SophiePower>().UpgradeValueBy(2);
	}

	protected override CardLocation GetResultLocationForCardPlay()
	{
		CardLocation result = base.GetResultLocationForCardPlay();
		if (result.pileType == PileType.Discard)
		{
			result.pileType = PileType.Hand;
		}
		return result;
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (play.Target != null)
		{
			await PlayerCmd.LoseGold(DynamicVars["GoldCost"].BaseValue, base.Owner);
			await PowerCmd.Apply<StrengthPower>(ctx, play.Target, -DynamicVars.Power<SophiePower>().BaseValue,
				base.Owner.Creature, null);
			await PowerCmd.Apply<SophiePower>(ctx, play.Target, DynamicVars.Power<SophiePower>().BaseValue,
				base.Owner.Creature, null);
		}
	}

	public Sophie() : base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy) {}
}