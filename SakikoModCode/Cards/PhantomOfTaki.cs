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

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class PhantomOfTaki : SakikoModBaseCard
{
	private readonly HashSet<SakikoCardTag> _sakikoTags = new() { SakikoCardTag.Phantom };
	protected override HashSet<SakikoCardTag> CanonicalSakikoTags => _sakikoTags;
	
	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(7, ValueProp.Move),
		new DynamicVar("AttackTimes", 2)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Addition);
			yield return HoverTipFactory.FromCard<Superiority>();
		}
	}

	protected override void OnUpgrade()
	{
		DynamicVars["AttackTimes"].UpgradeValueBy(1);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		if (play.Target != null)
		{
			AttackCommand command = DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
				.WithHitCount((int)DynamicVars["AttackTimes"].BaseValue).Targeting(play.Target);
			await command.Execute(ctx);
			foreach (var list in command.Results)
			{
				foreach (var result in list)
				{
					if (result.UnblockedDamage != 0)
					{
						await CreatureCmd.GainBlock(base.Owner.Creature, result.UnblockedDamage, ValueProp.Move, play);
					}
				}
			}
		}
		CardModel c = base.Owner.Creature.CombatState.CreateCard<Superiority>(base.Owner);
		await SakikoModCmd.InGameAdd(base.Owner.Creature, ctx, c, PileType.Discard);
	}

	public PhantomOfTaki() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) {}
}