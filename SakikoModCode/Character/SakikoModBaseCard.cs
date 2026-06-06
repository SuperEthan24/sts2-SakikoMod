using System.Reflection;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace SakikoMod.SakikoModCode.Character;

public abstract class SakikoModBaseCard : CustomCardModel
{
    private HashSet<SakikoCardTag>? _sakikoTags;
    protected virtual HashSet<SakikoCardTag> CanonicalSakikoTags => new HashSet<SakikoCardTag>();
    protected IEnumerable<SakikoCardTag> SakikoTags
    {
        get
        {
            return (IEnumerable<SakikoCardTag>) this._sakikoTags ?? (IEnumerable<SakikoCardTag>) (this._sakikoTags = this.CanonicalSakikoTags);
        }
    }

    protected SakikoModBaseCard(int baseCost, CardType type, CardRarity rarity, TargetType target,
        bool showInCardLibrary = true, bool autoAdd = true)
        : base(baseCost, type, rarity, target, showInCardLibrary, autoAdd) { }
    
    protected override void AddExtraArgsToDescription(LocString description)
    {
        base.AddExtraArgsToDescription(description);
//        FormDescription.AddTokens(this, description);
        // IfUpgraded 让 loc 用 SmartFormat 条件 {IfUpgraded:show:upText|baseText} 切换文字。
        // canonical model 的 IsUpgraded 是 false，所以卡库里看的是基础版；玩家升级后再访问的
        // 是 mutable instance，IsUpgraded 是 true，自动切到升级文本
        bool isUp = !IsCanonical && IsUpgraded;
        description.Add("IfUpgraded", isUp);

        // **必须包装成 IfUpgradedVar**——vanilla ShowIfUpgradedFormatter `isinst IfUpgradedVar` 检查
        // 类型，普通 bool 不匹配会抛 "No suitable Formatter could be found"（IL-verified）。
        // 用 (string, decimal) ctor 自定义 name，手动 set public field `upgradeDisplay`：
        //   Upgraded → 显示 :show: 的第 1 个分支
        //   Normal   → 显示 :show: 的第 2 个分支
        bool isInHand = !IsCanonical && Pile?.Type == PileType.Hand;
        var realVar = new IfUpgradedVar("ShowRealEffect", isInHand ? 0m : 1m)
        {
            upgradeDisplay = isInHand ? UpgradeDisplay.Normal : UpgradeDisplay.Upgraded,
        };
        description.Add(realVar);
    }
    
    /// <summary>
    /// 播 cast 动画 —— vanilla 标准 boilerplate（每张想要 cast 动画的技能/能力卡 OnPlay 第一句 await 这个）。
    /// 攻击卡不用：DamageCmd.Attack(...).Execute(...) 内部 AttackCommand.Execute 已自动触发 attack 动画。
    /// 纯获取格挡的卡也不用：vanilla DefendIronclad/DefendDefect 一致只播 block VFX，没 cast 动画。
    /// </summary>
    protected Task PlayCast()
        => CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
}