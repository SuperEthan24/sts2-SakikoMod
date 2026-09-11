using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class SymbolIiAir : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new PowerVar<ExtraTurnPower>(1),
		new PowerVar<SymbolIiAirPower>(1)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		EnergyCost.UpgradeBy(-1);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		await PowerCmd.Apply<ExtraTurnPower>(ctx, base.Owner.Creature, DynamicVars.Power<ExtraTurnPower>().BaseValue,
			base.Owner.Creature, this);
		await PowerCmd.Apply<SymbolIiAirPower>(ctx, base.Owner.Creature, DynamicVars.Power<SymbolIiAirPower>().BaseValue,
			base.Owner.Creature, this);
	}

	public SymbolIiAir() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {}
}