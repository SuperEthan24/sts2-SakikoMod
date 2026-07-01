using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class PhantomOfTomori : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new BlockVar(16, ValueProp.Move)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	private readonly HashSet<SakikoCardTag> _sakikoTags = new() { SakikoCardTag.Phantom };
	protected override HashSet<SakikoCardTag> CanonicalSakikoTags => _sakikoTags;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Addition);
			yield return HoverTipFactory.FromCard<Ignorance>();
			yield return HoverTipFactory.FromCard<GiantRock>();
		}
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		await CreatureCmd.GainBlock(base.Owner.Creature, DynamicVars.Block, play);
		CardModel rock = base.Owner.Creature.CombatState.CreateCard<GiantRock>(base.Owner);
		if (this.IsUpgraded)
		{
			CardCmd.Upgrade(rock);
		}

		await CardPileCmd.Add(rock, PileType.Hand);
		CardModel c = base.Owner.Creature.CombatState.CreateCard<Ignorance>(base.Owner);
		await SakikoModCmd.InGameAdd(base.Owner.Creature, ctx, c, PileType.Discard);
	}

	public PhantomOfTomori() : base(1, CardType.Skill, CardRarity.Common, TargetType.None) {}
}