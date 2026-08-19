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
public class ChoirSChoir : SakikoCharacterBaseCard
{
	protected override bool IsPlayable => base.Owner.Gold >= DynamicVars["GoldCost"].BaseValue;
	protected override bool ShouldGlowRedInternal => !IsPlayable;

	private readonly List<DynamicVar> _vars = new()
	{
		new GoldCostVar(15),
		new PowerVar<BlockRetainPower>(1)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars["GoldCost"].UpgradeValueBy(-5);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		await PlayerCmd.LoseGold(DynamicVars["GoldCost"].BaseValue, base.Owner);
		await PowerCmd.Apply<BlockRetainPower>(ctx, base.Owner.Creature,
			DynamicVars.Power<BlockRetainPower>().BaseValue, base.Owner.Creature, this);
	}

	public ChoirSChoir() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) {}
}