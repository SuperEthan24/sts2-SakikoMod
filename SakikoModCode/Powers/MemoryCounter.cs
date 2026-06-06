using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Powers;

public class MemoryCounter : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override bool AllowNegative => false;
    protected override bool IsVisibleInternal => false;
    
    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        try
        {
            if (creator != null && creator.Creature == base.Owner)
            {
                Flash();
                CardCmd.ApplyKeyword(card, SakikoModKeywords.Generated);
            }

            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            return Task.FromException(exception);
        }
    }
}