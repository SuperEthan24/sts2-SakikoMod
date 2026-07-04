using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using SakikoMod.SakikoModCode.Cards;
using SakikoMod.SakikoModCode.Relics;

namespace SakikoMod.SakikoModCode;

[HarmonyPatch(typeof(TouchOfOrobas), "RefinementUpgrades", MethodType.Getter)]
public static class TouchOfOrobasSakikoPatch
{
    [HarmonyPostfix]
    static void Postfix(ref Dictionary<ModelId, RelicModel> __result)
    {
        // WornRibbonRelic → ShinyRibbonRelic
        __result[ModelDb.Relic<WornRibbonRelic>().Id] =
            ModelDb.Relic<ShinyRibbonRelic>();
    }
}

[HarmonyPatch(typeof(ArchaicTooth), "TranscendenceUpgrades", MethodType.Getter)]
public static class ArchaicToothSakikoPatch
{
    [HarmonyPostfix]
    static void Postfix(ref Dictionary<ModelId, CardModel> __result)
    {
        // Oblivion → Symbol V Ether（古老牙齿把初始牌转化为先古版）
        __result[ModelDb.Card<Oblivionis>().Id] = ModelDb.Card<SymbolVEther>();
    }
}