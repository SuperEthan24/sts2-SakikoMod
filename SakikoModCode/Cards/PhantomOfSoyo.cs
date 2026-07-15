using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class PhantomOfSoyo : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new DamageVar(12, ValueProp.Move),
		new DynamicVar("AttackTimes", 2),
		new DynamicVar("ExtraAttack", 2)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	private readonly HashSet<SakikoCardTag> _sakikoTags = new() { SakikoCardTag.Phantom };
	protected override HashSet<SakikoCardTag> CanonicalSakikoTags => _sakikoTags;
	
	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Addition);
			yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Deletion);
			yield return HoverTipFactory.FromCard<Indifference>();
		}
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(4);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		decimal attackTimes = DynamicVars["AttackTimes"].BaseValue;
		CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
		prefs.ShouldGlowGold = (CardModel c) => (c.Type == CardType.Curse);
		CardModel? c = (await CardSelectCmd.FromHand(ctx, base.Owner, prefs, null, this)).FirstOrDefault();
		if (c != null)
		{
			if (c.Type == CardType.Curse)
			{
				await SakikoModCmd.InGameDelete(base.Owner.Creature, ctx, c);
				attackTimes += DynamicVars["ExtraAttack"].BaseValue;
			}
			else
			{
				await CardCmd.Exhaust(ctx, c);
			}
		}

		if (play.Target != null)
		{
			await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
				.FromCard(this, play).WithHitCount((int)attackTimes).Targeting(play.Target).Execute(ctx);
		}

		CardModel card = base.Owner.Creature.CombatState.CreateCard<Indifference>(base.Owner);
		await SakikoModCmd.InGameAdd(base.Owner.Creature, ctx, card, PileType.Discard);
	}

	public PhantomOfSoyo() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) {}
}