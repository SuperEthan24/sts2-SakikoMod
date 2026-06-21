using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace SakikoMod.SakikoModCode.Powers;

public class DecrescentPower : CustomPowerModel
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Single;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new DynamicVar("Value", 6)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromPower<SakikoDarkPower>();
        }
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card.Type is CardType.Attack)
        {
            await PowerCmd.Apply<SakikoDarkPower>(ctx, base.Owner, DynamicVars["Value"].BaseValue, base.Owner,
                play.Card);
        }
    }
}