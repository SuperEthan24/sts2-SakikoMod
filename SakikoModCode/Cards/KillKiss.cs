using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class KillKiss : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(20, ValueProp.Move),
		new PowerVar<ExtraTurnPower>(1)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Retain };
	public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPower<ExtraTurnPower>();
			yield return HoverTipFactory.Static(StaticHoverTip.Fatal);
		}
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(10);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (play.Target != null)
		{
			bool shouldTriggerFatal = play.Target.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
			AttackCommand command = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play)
				.Targeting(play.Target).Execute(ctx);
			if (shouldTriggerFatal && command.Results.SelectMany((List<DamageResult> r) => r)
				    .Any((DamageResult r) => r.WasTargetKilled))
			{
				await PowerCmd.Apply<ExtraTurnPower>(ctx, base.Owner.Creature,
					DynamicVars.Power<ExtraTurnPower>().BaseValue, base.Owner.Creature, this);
			}
		}
	}

	public KillKiss() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {}
}