using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class PhantomOfMustumi : SakikoModBaseCard
{
	
	private readonly List<DynamicVar> _vars = new()
	{
		new CardsVar(3)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	private readonly HashSet<SakikoCardTag> _sakikoTags = new() { SakikoCardTag.Phantom };
	protected override HashSet<SakikoCardTag> CanonicalSakikoTags => _sakikoTags;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Addition);
			yield return HoverTipFactory.FromCard<Arrogance>();
		}
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		await CardPileCmd.Draw(ctx, DynamicVars.Cards.BaseValue, base.Owner);
		CardModel c = base.Owner.Creature.CombatState.CreateCard<Arrogance>(base.Owner);
		await SakikoModCmd.InGameAdd(base.Owner.Creature, ctx, c, PileType.Discard);
	}

	public PhantomOfMustumi() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) {}
}