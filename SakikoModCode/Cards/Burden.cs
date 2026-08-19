using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Burden : SakikoCharacterBaseCard
{
	public override bool CanBeGeneratedInCombat => false;
	public override bool CanBeGeneratedByModifiers => false;
	protected override bool IsPlayable => false;
	public override int MaxUpgradeLevel => 0;
	public override bool HasOnDeletionEffect => true;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Deletion);
			yield return HoverTipFactory.FromCard<Exhaustion>();
		}
	}
	
	private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Eternal };
	public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

	public override async Task<bool> OnDeletion(PlayerChoiceContext ctx)
	{
		if (base.Owner.Creature.CombatState == null) return false;
		await CardCmd.Discard(ctx, this);
		CardModel exhaustion = base.Owner.Creature.CombatState.CreateCard<Exhaustion>(base.Owner);
		CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(exhaustion, PileType.Hand));
		return true;
	}

	public Burden() : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None) {}
}