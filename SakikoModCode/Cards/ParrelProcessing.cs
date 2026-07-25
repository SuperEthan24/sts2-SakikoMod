using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class ParrelProcessing : SakikoCharacterBaseCard
{
	protected override bool ShouldGlowGoldInternal => base.Owner.Creature.GetPower<ExtraTurnPower>() != null;

	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(9, ValueProp.Move),
		new BlockVar(9, ValueProp.Move)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(3);
		DynamicVars.Block.UpgradeValueBy(3);
	}

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<ExtraTurnPower>();
		}
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (play.Target != null)
		{
			await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
				.FromCard(this, play).Targeting(play.Target).Execute(ctx);
		}
		if (base.Owner.Creature.GetPower<ExtraTurnPower>() != null)
		{
			await CreatureCmd.GainBlock(Owner.Creature,
				DynamicVars.Block.BaseValue, ValueProp.Move, play);
		}
	}

	public ParrelProcessing() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) {}
}