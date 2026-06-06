using System.Diagnostics.Tracing;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace SakikoMod.SakikoModCode.Powers;

public class CardDeletePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override bool AllowNegative => false;
    protected override bool IsVisibleInternal => false;

    [SavedProperty] private HashSet<CardModel> _cards = new HashSet<CardModel>();

    public void DeleteCard(CardModel card)
    {
        _cards.Add(card);
    }
    
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        foreach (CardModel c in _cards)
        {
            if (c.DeckVersion != null)
            {
                await CardPileCmd.RemoveFromDeck(c.DeckVersion, false);
            }
        }
        _cards.Clear();
    }
}