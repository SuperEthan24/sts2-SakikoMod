using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.DynamicVars;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class SalaryOnWeekend : SakikoModBaseCard
{
    private readonly List<DynamicVar> _vars = new()
    {
        new GoldEarnVar(5),
        new PowerVar<GoldNextTurnPower>(15)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromPower<GoldNextTurnPower>();
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["GoldNextTurnPower"].UpgradeValueBy(5);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PlayerCmd.GainGold(DynamicVars["GoldEarn"].BaseValue, base.Owner);
        await PowerCmd.Apply<GoldNextTurnPower>(ctx, base.Owner.Creature, DynamicVars["GoldNextTurnPower"].BaseValue,
            base.Owner.Creature, this);
    }

    public SalaryOnWeekend() : base(1, CardType.Skill, CardRarity.Common, TargetType.None) {}
}