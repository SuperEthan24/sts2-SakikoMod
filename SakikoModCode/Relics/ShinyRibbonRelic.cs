using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Saves.Runs;
using SakikoMod.SakikoModCode.Cards;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;
using SakikoMod.SakikoModCode.Rewards;

namespace SakikoMod.SakikoModCode.Relics;

[Pool(typeof(SakikoModRelicPool))]
public class ShinyRibbonRelic : CustomRelicModel
{
    [SavedProperty]
    private bool DidCombatStart { get; set; }
    
    public override RelicRarity Rarity => RelicRarity.Starter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromCard<Nihil>(true);
            yield return HoverTipFactory.FromCard<Desire>(true);
        }
    }
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner) return;
        if (DidCombatStart) return;
        if (Owner?.Creature?.CombatState == null) return;

        DidCombatStart = true;

        // 一次性 per-combat 初始化
        await CombatCounters.ResetThisCombat(ctx, player);

        var combatState = player.Creature.CombatState;
        if (combatState == null) return;  // 上面已 guard 过，但编译器不认
        
        await PowerCmd.Apply<CardDeletePower>(ctx, player.Creature, 1, player.Creature, null, true);
        await PowerCmd.Apply<CardAddPower>(ctx, player.Creature, 1, player.Creature, null, true);
        
        await PowerCmd.Apply<CrescentPower>(ctx, player.Creature, 1, player.Creature, null);
        await PowerCmd.Apply<ForwardPower>(ctx, player.Creature, 1, player.Creature, null);
    }
    
    public override async Task AfterCombatVictory(MegaCrit.Sts2.Core.Rooms.CombatRoom room)
    {
        DidCombatStart = false;
        // 战斗结束 reset 计数器（power 形式现在 → PowerCmd.Remove 是 async，方法签名跟着 async）
        // 理论上 vanilla 战斗结束自动清所有 power，这里加一层保险
        if (Owner != null)
        {
            await CombatCounters.ResetThisCombat(null, Owner);
            room.AddExtraReward(base.Owner, new RibbonReward(base.Owner, true));
            Flash();
        }
    }
}