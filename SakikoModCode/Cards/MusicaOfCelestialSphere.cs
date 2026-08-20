using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class MusicaOfCelestialSphere : SakikoCharacterBaseCard
{
	protected override bool IsPlayable => base.Owner.PlayerCombatState?.Hand.Cards.Where((CardModel c) => c != this &&
		c.CanPlay()).Count() == 0;

	protected override bool ShouldGlowGoldInternal => IsPlayable;

	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(12, ValueProp.Move),
		new RepeatVar(3)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(3);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		var cs = base.Owner.Creature.CombatState;
		if (cs != null)
		{
			await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play)
				.WithHitCount(DynamicVars.Repeat.IntValue).TargetingAllOpponents(cs).Execute(ctx);
		}
	}

	public MusicaOfCelestialSphere() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) {}
}