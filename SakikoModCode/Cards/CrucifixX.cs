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
public class CrucifixX : SakikoCharacterBaseCard
{
    protected override bool HasEnergyCostX => true;
    protected override bool ShouldGlowGoldInternal => base.Owner.PlayerCombatState.Energy >= base.DynamicVars.Energy.IntValue;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(12, ValueProp.Move),
        new EnergyVar(4)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    protected override void AddExtraArgsToDescription(MegaCrit.Sts2.Core.Localization.LocString description)
    {
        base.AddExtraArgsToDescription(description);
        // X 预览 = 当前可用能量。canonical（卡库）时 Owner getter 直接抛 → 必须先短路
        if (!IsInCombat)
        {
            description.Add("XAttack", (decimal)0);
        }
        else
        {
            int x = (Owner?.PlayerCombatState?.Energy ?? 0);
            int atkMult = (ShouldGlowGoldInternal ? (2 * x) : x);
            description.Add("XAttack", variable:(decimal)atkMult);
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);               // 10 → 12
    }
    
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        int num = ResolveEnergyXValue();
        if (num <= 0) return;
        if (num >= base.DynamicVars.Energy.IntValue)
        {
            num *= 2;
        }
        var cs = Owner.Creature.CombatState;
        // 单 AttackCommand + WithHitCount + TargetingRandomOpponents(allowDuplicates=true)
        // → 每次 hit 框架内部独立选随机敌人（可重复）；力量/活力 modifier 算一次但应用到每次 hit
        if (cs != null && cs.HittableEnemies.Count > 0 && num > 0)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, play).TargetingRandomOpponents(cs, allowDuplicates: true)
                .WithHitCount(num).Execute(ctx);
        }
    }
    
    public CrucifixX() : base(0, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy) { }
}