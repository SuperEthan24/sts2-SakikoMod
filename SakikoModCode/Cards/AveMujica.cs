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
public class AveMujica : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new PowerVar<AveMujicaPower>(1)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<AveMujicaPower>();
			yield return HoverTipFactory.FromKeyword(CardKeyword.Exhaust);
		}
	}

	protected override void OnUpgrade()
	{
		EnergyCost.UpgradeBy(-1);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay _)
	{
		await PowerCmd.Apply<AveMujicaPower>(ctx, base.Owner.Creature, DynamicVars.Power<AveMujicaPower>().BaseValue,
			base.Owner.Creature, this);
	}

	public AveMujica() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self) {}
}