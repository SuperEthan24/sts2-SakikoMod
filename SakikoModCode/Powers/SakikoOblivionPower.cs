using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace SakikoMod.SakikoModCode.Powers;

public class SakikoOblivionPower: CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (base.Owner.Player != null)
            for (int i = 0; i < base.Amount; i++)
            {
                room.AddExtraReward(base.Owner.Player, new CardRemovalReward(base.Owner.Player));
            }
        return Task.CompletedTask;
    }
}