using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Crychic : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new PowerVar<CrychicPower>(2)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars.Power<CrychicPower>().UpgradeValueBy(1);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay _)
	{
		await PowerCmd.Apply<CrychicPower>(ctx, base.Owner.Creature, DynamicVars.Power<CrychicPower>().BaseValue,
			base.Owner.Creature, this);
	}

	public Crychic() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) {}
}