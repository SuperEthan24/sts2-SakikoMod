using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class SymbolVEther : SakikoModBaseCard
{
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;

    private int _currentAttackTimes = 1;
    [SavedProperty]
    public int CurrentAttackTimes
    {
        get => _currentAttackTimes;
        set
        {
            _currentAttackTimes = value;
            DynamicVars["AttackTimes"].BaseValue = _currentAttackTimes;
        }
    }

    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(15, ValueProp.Move),
        new DynamicVar("AttackTimes", 1),
        new DynamicVar("Deletion", 0)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Exhaust, CardKeyword.Eternal };
    public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromPower<SakikoOblivionPower>();
            yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Deletion);
        }
    }
    
    protected override void AddExtraArgsToDescription(MegaCrit.Sts2.Core.Localization.LocString description)
    {
        base.AddExtraArgsToDescription(description);
        // X 预览 = 当前可用能量。canonical（卡库）时 Owner getter 直接抛 → 必须先短路
        if (IsCanonical)
        {
            description.Add("XAttack", (decimal)1);
        }
        else if (IsInCombat)
        {
            description.Add("XAttack", DynamicVars["AttackTimes"].BaseValue + DynamicVars["Deletion"].BaseValue);
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
        CurrentAttackTimes++;
        await base.BeforeCardRemoved(card);
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext ctx, CardModel card, bool causedByEthereal)
    {
        if (card.Keywords.Contains(SakikoModKeywords.ToBeDeleted))
        {
            DynamicVars["Deletion"].BaseValue++;
        }
        await base.AfterCardExhausted(ctx, card, causedByEthereal);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<SakikoOblivionPower>(ctx, base.Owner.Creature, 1m, base.Owner.Creature, this);
        var cs = Owner.Creature.CombatState;
        int num = (int)(CurrentAttackTimes + DynamicVars["Deletion"].BaseValue);
        if (cs != null && cs.HittableEnemies.Count > 0)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(cs)
                .WithHitCount(num).Execute(ctx);
        }
    }

    public SymbolVEther() : base(2, CardType.Attack, CardRarity.Ancient, TargetType.AllEnemies)
    {
        DynamicVars["AttackTimes"].BaseValue = CurrentAttackTimes;
    }
}