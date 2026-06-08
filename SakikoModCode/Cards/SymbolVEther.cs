using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class SymbolVEther : SakikoModBaseCard
{
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(15, ValueProp.Move),
        new DynamicVar("AttackTimes", 1)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Exhaust };
    public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromPower<SakikoOblivionPower>();
        }
    }
    
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
            description.Add("XAttack", DynamicVars["AttackTimes"].BaseValue);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override async Task BeforeCardRemoved(CardModel card)
    {
        DynamicVars["AttackTimes"].BaseValue++;
        await base.BeforeCardRemoved(card);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<SakikoOblivionPower>(ctx, base.Owner.Creature, 1m, base.Owner.Creature, this);
        var cs = Owner.Creature.CombatState;
        for (int i = 0; i < DynamicVars["AttackTimes"].BaseValue; i++)
        {
            if (cs != null && cs.HittableEnemies.Count > 0)
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this).TargetingAllOpponents(cs).Execute(ctx);
            }
            else
            {
                break;
            }
        }
    }

    public SymbolVEther() : base(2, CardType.Attack, CardRarity.Ancient, TargetType.AllEnemies) { }
}