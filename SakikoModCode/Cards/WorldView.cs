using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class WorldView : SakikoCharacterBaseCard
{
	protected override bool IsPlayable => base.Owner.Gold >= DynamicVars["GoldCost"].BaseValue;
	protected override bool ShouldGlowRedInternal => !IsPlayable;

	private readonly List<DynamicVar> _vars = new()
	{
		new GoldCostVar(10),
		new PowerVar<WeakPower>(1),
		new PowerVar<VulnerablePower>(1)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars.Power<WeakPower>().UpgradeValueBy(1);
		DynamicVars.Power<VulnerablePower>().UpgradeValueBy(1);
	}

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<WeakPower>();
			yield return HoverTipFactory.FromPower<VulnerablePower>();
		}
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (base.Owner.Creature.CombatState?.Enemies != null)
		{
			await PlayerCmd.LoseGold(DynamicVars["GoldCost"].BaseValue, base.Owner);
			foreach (Creature c in base.Owner.Creature.CombatState.Enemies)
			{
				await PowerCmd.Apply<WeakPower>(ctx, c, DynamicVars.Power<WeakPower>().BaseValue, base.Owner.Creature,
					this);
				await PowerCmd.Apply<VulnerablePower>(ctx, c, DynamicVars.Power<VulnerablePower>().BaseValue, base.Owner.Creature,
					this);
			}
		}
	}

	public WorldView() : base(2, CardType.Skill, CardRarity.Common, TargetType.AllEnemies) {}
}