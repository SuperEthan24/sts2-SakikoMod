using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace SakikoMod.SakikoModCode;

public class GoldCostVar : DynamicVar
{
    public GoldCostVar(int baseValue) : base("GoldCost", baseValue) { }
}

public class GoldEarnVar : DynamicVar
{
    public GoldEarnVar(int baseValue) : base("GoldEarn", baseValue) { }
}