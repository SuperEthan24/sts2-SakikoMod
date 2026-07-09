using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class TestGainPotion : SakikoCharacterBaseCard
{
	public override bool CanBeGeneratedInCombat => false;
	public override bool CanBeGeneratedByModifiers => false;
	public override int MaxUpgradeLevel => 0;
	protected override bool ShouldGlowRedInternal => !IsPlayable;
	protected override bool IsPlayable => base.Owner.HasOpenPotionSlots;

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		List<PotionCards> cards =
		[
			base.Owner.RunState.CreateCard<PotionCards>(base.Owner),
			base.Owner.RunState.CreateCard<PotionCards>(base.Owner),
			base.Owner.RunState.CreateCard<PotionCards>(base.Owner)
		];
		for (int i = 0; i < 3; i++)
		{
			PotionModel? p = base.Owner.RunState.Rng.CombatCardGeneration.NextItem(PotionFactory.GetPotionOptions(base.Owner));
			if (p != null)
			{
				int? index = ModelDb.AllPotions.Select((item, idx) => new { item, idx })
					.FirstOrDefault(x => x.item == p)?.idx;
				if (index != null)
				{
					cards[i].DynamicVars["Potion"].BaseValue = (decimal)index;
				}
			}
		}
		CardModel? cardModel = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), cards, base.Owner);
		if (cardModel != null)
		{
			await ((KnowledgeDemon.IChoosable)cardModel).OnChosen();
		}
	}

	public TestGainPotion() : base(1, CardType.Skill, CardRarity.Common, TargetType.None) {}
}