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
        if (!creature.HasPower<CardAddPower>())
        {
            await PowerCmd.Apply<CardAddPower>(ctx, creature, 1, creature, null);
        }
        if (!creature.HasPower<CardDeletePower>())
        {
            await PowerCmd.Apply<CardDeletePower>(ctx, creature, 1, creature, null);
        }

        CardCmd.ApplyKeyword(cardModel, SakikoModKeywords.ToBeDeleted);
        if (!cardModel.Keywords.Contains(CardKeyword.Eternal))
        {
            if (cardModel.Keywords.Contains(SakikoModKeywords.ToBeAdded))
            {
                CardCmd.RemoveKeyword(cardModel, SakikoModKeywords.ToBeAdded);
                creature.GetPower<CardAddPower>().DeleteCard(cardModel);
            }
            else if (cardModel.DeckVersion != null)
            {
                creature.GetPower<CardDeletePower>().DeleteCard(cardModel);
            }
        }

        await CardCmd.Exhaust(ctx, cardModel, skipVisuals:skipVisuals);
        await CardPileCmd.RemoveFromCombat(cardModel, skipVisuals);
    }
    public static async Task InGameDelete(Creature creature, PlayerChoiceContext ctx, IEnumerable<CardModel> cardModels,
        bool skipVisuals = false)
    {
        if (!creature.HasPower<CardAddPower>())
        {
            await PowerCmd.Apply<CardAddPower>(ctx, creature, 1, creature, null);
        }
        if (!creature.HasPower<CardDeletePower>())
        {
            await PowerCmd.Apply<CardDeletePower>(ctx, creature, 1, creature, null);
        }

        foreach (var c in cardModels.ToList())
        {
            CardCmd.ApplyKeyword(c, SakikoModKeywords.ToBeDeleted);
            if (!c.Keywords.Contains(CardKeyword.Eternal))
            {
                if (c.Keywords.Contains(SakikoModKeywords.ToBeAdded))
                {
                    CardCmd.RemoveKeyword(c, SakikoModKeywords.ToBeAdded);
                    creature.GetPower<CardAddPower>().DeleteCard(c);
                }
                else if (c.DeckVersion != null)
                {
                    creature.GetPower<CardDeletePower>().DeleteCard(c);
                }
            }

            await CardCmd.Exhaust(ctx, c, skipVisuals:skipVisuals);
            await CardPileCmd.RemoveFromCombat(c, skipVisuals);
        }
    }

    public static async Task InGameAdd(Creature creature, PlayerChoiceContext ctx, CardModel cardModel,
        PileType pileType, bool skipVisuals = false)
    {
        if (!creature.HasPower<CardAddPower>())
        {
            await PowerCmd.Apply<CardAddPower>(ctx, creature, 1, creature, null);
        }
        CardCmd.ApplyKeyword(cardModel, SakikoModKeywords.ToBeAdded);
        creature.GetPower<CardAddPower>().AddCard(cardModel);
        if (pileType != PileType.None)
        {
            if (skipVisuals)
            {
                await CardPileCmd.Add(cardModel, pileType,
                    (pileType == PileType.Draw ? CardPilePosition.Random : CardPilePosition.Bottom), skipVisuals:true);
            }
            else
            {
                CardCmd.PreviewCardPileAdd(
                    await CardPileCmd.Add(cardModel, pileType,
                        (pileType == PileType.Draw ? CardPilePosition.Random : CardPilePosition.Bottom)), 2f);
            }
        }
        else
        {
            await Cmd.Wait(0.01f);
        }
    }
}