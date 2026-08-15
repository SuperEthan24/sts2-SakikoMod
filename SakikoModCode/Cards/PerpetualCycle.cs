using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class PerpetualCycle : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new PowerVar<PerpetualCyclePower>(1)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		CardCmd.ApplyKeyword(this, CardKeyword.Innate);
	}

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<PerpetualCyclePower>();
		}
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay _)
	{
		await PowerCmd.Apply<PerpetualCyclePower>(ctx, base.Owner.Creature,
			DynamicVars.Power<PerpetualCyclePower>().BaseValue, base.Owner.Creature, this);
	}

	public PerpetualCycle() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) {}
}