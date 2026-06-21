using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class Indifference : SakikoModBaseCard
{
	public override int MaxUpgradeLevel => 0;
	public override bool HasTurnEndInHandEffect => true;
	public override bool CanBeGeneratedInCombat => false;
	public override bool CanBeGeneratedByModifiers => false;
	
	private readonly List<DynamicVar> _vars = new()
	{
		new CardsVar(1)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Ethereal, CardKeyword.Exhaust };
	public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

	protected override async Task OnTurnEndInHand(PlayerChoiceContext ctx)
	{
		await PowerCmd.Apply<IndifferencePower>(ctx, base.Owner.Creature, DynamicVars.Cards.BaseValue,
			base.Owner.Creature, null);
	}

	public Indifference() : base(1, CardType.Curse, CardRarity.Curse, TargetType.None) {}
}