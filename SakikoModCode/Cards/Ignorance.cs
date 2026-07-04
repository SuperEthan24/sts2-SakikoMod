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

[Pool(typeof(SakikoCharacterCardPool))]
public class Ignorance : SakikoCharacterBaseCard
{
	public override int MaxUpgradeLevel => 0;
	public override bool CanBeGeneratedInCombat => false;
	public override bool CanBeGeneratedByModifiers => false;
	
	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(2, ValueProp.Unpowered)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Ethereal };
	public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

	public override async Task AfterSideTurnEnd(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side != base.Owner.Creature.Side)
		{
			return;
		}

		if (this.Pile is { Type: PileType.Exhaust })
		{
			await CreatureCmd.Damage(ctx, base.Owner.Creature, DynamicVars.Damage, base.Owner.Creature);
		}
	}

	public Ignorance() : base(1, CardType.Curse, CardRarity.Curse, TargetType.None) { }
}