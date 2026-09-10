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
public class StageSetting : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new PowerVar<StageSettingPower>(4)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<StageSettingPower>();
		}
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Power<StageSettingPower>().UpgradeValueBy(2);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		await PowerCmd.Apply<StrengthPower>(ctx, base.Owner.Creature,
			DynamicVars.Power<StageSettingPower>().BaseValue, base.Owner.Creature, this);
		await PowerCmd.Apply<StageSettingPower>(ctx, base.Owner.Creature,
			DynamicVars.Power<StageSettingPower>().BaseValue, base.Owner.Creature, this);
	}

	public StageSetting() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) {}
}