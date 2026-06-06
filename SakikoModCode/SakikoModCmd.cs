using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode;

public static class SakikoModCmd
{
    public static async Task InGameDelete(Creature creature, PlayerChoiceContext ctx, CardModel cardModel,
        bool skipVisuals = false)
    {

        if (!(cardModel.Keywords.Contains(SakikoModKeywords.Generated) || cardModel.Keywords.Contains(CardKeyword.Eternal)))
        {
            CardCmd.ApplyKeyword(cardModel, SakikoModKeywords.ToBeDeleted);
            CardCmd.ApplyKeyword(cardModel, CardKeyword.Unplayable);
            CardCmd.ApplyKeyword(cardModel, CardKeyword.Exhaust);
            if (cardModel.Keywords.Contains(SakikoModKeywords.ToBeAdded))
            {
                CardCmd.RemoveKeyword(cardModel, SakikoModKeywords.ToBeAdded);
                creature.GetPower<CardAddPower>().DeleteCard(cardModel);
            }
            else
            {
                creature.GetPower<CardDeletePower>().DeleteCard(cardModel);
            }
        }
        await CardCmd.Exhaust(ctx, cardModel, false, skipVisuals);
    }

    public static async Task InGameAdd(Creature creature, PlayerChoiceContext ctx, CardModel cardModel,
        bool skipVisuals = false)
    {
        CardCmd.ApplyKeyword(cardModel, SakikoModKeywords.ToBeAdded);
        creature.GetPower<CardAddPower>().AddCard(cardModel);
        await Cmd.Wait(0.01f);
    }
}