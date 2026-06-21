using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class Arrogance : SakikoModBaseCard
{
	public override int MaxUpgradeLevel => 0;
	public override bool HasTurnEndInHandEffect => true;
	public override bool CanBeGeneratedInCombat => false;
	public override bool CanBeGeneratedByModifiers => false;
	
	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(2, ValueProp.Unpowered)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Unplayable, CardKeyword.Ethereal };
	public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

	public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
	{
		if (this.Pile != null && this.Pile.Type == PileType.Hand)
			await CreatureCmd.Damage(ctx, base.Owner.Creature, DynamicVars.Damage, this);
	}

	public Arrogance() : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None) {}
}