using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Impromptu : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new EnergyVar(1),
		new CardsVar(2)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Retain, SakikoModKeywords.Contingency };
	public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

	protected override void OnUpgrade()
	{
		DynamicVars.Energy.UpgradeValueBy(1);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay _)
	{
		await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, base.Owner);
		await CardPileCmd.Draw(ctx, base.Owner);
	}

	public Impromptu() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {}
}