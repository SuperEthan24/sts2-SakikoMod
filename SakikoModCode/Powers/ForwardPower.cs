using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Powers;

public class ForwardPower : CustomPowerModel
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task BeforeSideTurnEndEarly(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != base.Owner.Side) return;
        await SakikoModCmd.TimeForward(base.Owner, ctx, false);
    }
}