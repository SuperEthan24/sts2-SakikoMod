using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Exhaustion : SakikoCharacterBaseCard
{
	public override bool CanBeGeneratedInCombat => false;
	public override bool CanBeGeneratedByModifiers => false;
	
	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(10, ValueProp.Move),
		new DamageVar("SelfDamage", 4, ValueProp.Unpowered)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
	private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Exhaust };
	public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(6);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (play.Target != null)
		{
			await DamageCmd.Attack(DynamicVars["SelfDamage"].BaseValue)
				.FromCard(this, play).Targeting(base.Owner.Creature).Execute(ctx);
			await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
				.FromCard(this, play).Targeting(play.Target).Execute(ctx);
		}
	}

	public Exhaustion() : base(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy) {}
}