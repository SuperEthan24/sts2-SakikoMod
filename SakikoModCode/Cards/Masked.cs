using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Masked : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new PowerVar<MaskedPower>(20)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars.Power<MaskedPower>().UpgradeValueBy(10);
	}

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<MaskedPower>();
		}
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		await PowerCmd.Apply<MaskedPower>(ctx, base.Owner.Creature, DynamicVars.Power<MaskedPower>().BaseValue,
			base.Owner.Creature, this);
	}

	public Masked() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) {}
}