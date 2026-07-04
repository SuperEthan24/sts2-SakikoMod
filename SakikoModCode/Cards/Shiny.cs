using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Shiny : SakikoCharacterBaseCard
{
    protected override bool ShouldGlowGoldInternal => base.Owner.PlayerCombatState.Energy <= base.DynamicVars["Limit"].IntValue;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(12, ValueProp.Move),
        new EnergyVar(1),
        new DynamicVar("Limit", 3)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    public override TargetType TargetType => TargetType.AnyEnemy;

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, play).Targeting(play.Target).Execute(ctx);
        }

        if (base.Owner.PlayerCombatState.Energy + base.EnergyCost.GetAmountToSpend() <= base.DynamicVars["Limit"].IntValue)
        {
            await PlayerCmd.GainEnergy((int)base.DynamicVars.Energy.BaseValue, base.Owner);
        }
    }
    
    public Shiny() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
}