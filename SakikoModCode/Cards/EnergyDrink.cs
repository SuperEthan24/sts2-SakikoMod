using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class EnergyDrink : SakikoCharacterBaseCard
{
    protected override bool ShouldGlowRedInternal => !IsPlayable;
    protected override bool IsPlayable => base.Owner.Gold >= DynamicVars["GoldCost"].BaseValue;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new GoldCostVar(15),
        new EnergyVar(2)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner != null)
        {
            await PlayerCmd.LoseGold(DynamicVars["GoldCost"].BaseValue, base.Owner);
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, base.Owner);
        }
    }

    public EnergyDrink() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }
}