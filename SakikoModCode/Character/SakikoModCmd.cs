using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Character;

public static class SakikoModCmd
{
    public static async Task InGameDelete(Creature creature, PlayerChoiceContext ctx, CardModel cardModel,
        bool skipVisuals = false)
    {

        if (!(cardModel.Keywords.Contains(SakikoModKeywords.Generated) || cardModel.Keywords.Contains(CardKeyword.Eternal)))
        {
            CardCmd.ApplyKeyword(cardModel, SakikoModKeywords.ToBeDeleted);
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
        await CardPileCmd.RemoveFromCombat(cardModel, true);
    }

    public static async Task InGameAdd(Creature creature, PlayerChoiceContext ctx, CardModel cardModel,
        PileType pileType, bool skipVisuals = false)
    {

        CardCmd.ApplyKeyword(cardModel, SakikoModKeywords.ToBeAdded);
        creature.GetPower<CardAddPower>().AddCard(cardModel);
        if (pileType != PileType.None)
        {
            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.Add(cardModel, pileType,
                    (pileType == PileType.Draw ? CardPilePosition.Random : CardPilePosition.Bottom)), 2f);
        }
        else
        {
            await Cmd.Wait(0.01f);
        }
    }
}