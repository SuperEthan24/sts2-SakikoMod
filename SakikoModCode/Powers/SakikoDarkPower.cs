using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace SakikoMod.SakikoModCode.Powers;

public class SakikoDarkPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;

    private readonly List<DynamicVar> _vars = new()
    {
        new CardsVar(0),
        new DynamicVar("MaximumStack", 30)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext ctx, PowerModel power, decimal amount,
        Creature? applier, CardModel? cardSource)
    {
        if (base.Owner.HasPower<HaruhikagePower>())
        {
            DynamicVars["MaximumStack"].BaseValue = 60;
        }
        if (Amount > DynamicVars["MaximumStack"].BaseValue)
        {
            await PowerCmd.ModifyAmount(ctx, this, DynamicVars["MaximumStack"].BaseValue, applier, cardSource);
        }

        DynamicVars.Cards.BaseValue = Amount / 15;
    }
    
    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != base.Owner.Player)
        {
            return count;
        }
        if (base.AmountOnTurnStart == 0)
        {
            return count;
        }
        return count + DynamicVars.Cards.BaseValue;
    }
}