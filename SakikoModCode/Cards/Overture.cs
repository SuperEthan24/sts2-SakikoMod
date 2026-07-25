using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Overture : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new EnergyVar(6),
		new CardsVar(8)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
	private readonly HashSet<CardKeyword> _keywords = new()
	{
		CardKeyword.Exhaust
	};
	public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

	protected override void OnUpgrade()
	{
		DynamicVars.Energy.UpgradeValueBy(1);
		DynamicVars.Cards.UpgradeValueBy(1);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (base.Owner.PlayerCombatState?.Energy < DynamicVars.Energy.BaseValue)
		{
			await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue - base.Owner.PlayerCombatState.Energy, base.Owner);
		}

		while (base.Owner.PlayerCombatState?.Hand.Cards.Count < DynamicVars.Cards.BaseValue)
		{
			await CardPileCmd.Draw(ctx, base.Owner);
		}
	}

	public Overture() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {}
}