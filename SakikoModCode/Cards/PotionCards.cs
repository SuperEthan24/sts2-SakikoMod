using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class PotionCards : SakikoCharacterBaseCard, KnowledgeDemon.IChoosable
{
	public override bool CanBeGeneratedInCombat => false;
	public override bool CanBeGeneratedByModifiers => false;
	public override int MaxUpgradeLevel => 0;
	
	public override string PortraitPath => ModelDb.AllPotions.ElementAt(DynamicVars["Potion"].IntValue).ImagePath;

	private readonly List<DynamicVar> _vars = new()
	{
		new DynamicVar("Potion", 0)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void AddExtraArgsToDescription(LocString description)
	{
		base.AddExtraArgsToDescription(description);
		description.Add("PotionName", ModelDb.AllPotions.ElementAt(DynamicVars["Potion"].IntValue).Title);
		description.Add("PotionDescription",
			ModelDb.AllPotions.ElementAt(DynamicVars["Potion"].IntValue).DynamicDescription);
	}

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromPotion(ModelDb.AllPotions.ElementAt(DynamicVars["Potion"].IntValue));
			foreach (var extraHoverTip in ModelDb.AllPotions.ElementAt(DynamicVars["Potion"].IntValue).ExtraHoverTips)
				yield return extraHoverTip;
		}
	}

	public async Task OnChosen()
	{
		PotionModel potion = ModelDb.AllPotions.ElementAt(DynamicVars["Potion"].IntValue).ToMutable();
		await PotionCmd.TryToProcure(potion, base.Owner);
	}

	public PotionCards() : base(-1, CardType.Status, CardRarity.Status, TargetType.None) {}
}