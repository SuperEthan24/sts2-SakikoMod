using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode;

public static class CombatCounters
{
    /// <summary>
    /// 战斗开始 (`AfterPlayerTurnStart` 第 1 回合) + 战斗结束 (`AfterCombatVictory`) 都调。
    /// 显式 Remove per-combat power 防残留（vanilla 战斗结束理应自动清，加一层保险）。
    /// 注：async，callsite 必须 await。
    /// </summary>
    public static async Task ResetThisCombat(PlayerChoiceContext? ctx, Player p)
    {
        var c = p?.Creature;
        if (c == null) return;
        if (c.HasPower<CardDeletePower>())
            await PowerCmd.Remove<CardDeletePower>(c);
        if (c.HasPower<CardAddPower>())
            await PowerCmd.Remove<CardAddPower>(c);
    }
}