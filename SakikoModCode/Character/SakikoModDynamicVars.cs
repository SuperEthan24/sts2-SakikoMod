using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace SakikoMod.SakikoModCode.Character;

public class GoldCostVar : DynamicVar
{
    public GoldCostVar(int baseValue) : base("GoldCost", baseValue) { }
}

public class GoldEarnVar : DynamicVar
{
    public GoldEarnVar(int baseValue) : base("GoldEarn", baseValue) { }
}
public class BackwardVar : DynamicVar
{
    public BackwardVar(int baseValue) : base("Backward", baseValue) { }
}
public class ForwardVar : DynamicVar
{
    public ForwardVar(int baseValue) : base("Forward", baseValue) { }
}