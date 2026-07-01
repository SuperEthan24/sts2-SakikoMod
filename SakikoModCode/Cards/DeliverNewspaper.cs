using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class DeliverNewspaper : SakikoCharacterBaseCard
{
    private readonly List<DynamicVar> _vars = new()
    {
        new GoldEarnVar(10)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    protected override void OnUpgrade()
    {
        DynamicVars["GoldEarn"].UpgradeValueBy(5);               // 10 → 15
    }
    
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        if (base.Owner != null)
        {
            await PlayerCmd.GainGold(DynamicVars["GoldEarn"].BaseValue, base.Owner);
        }
    }
    
    public DeliverNewspaper() : base(1, CardType.Skill, CardRarity.Basic, TargetType.None) { }
}