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

        await CardCmd.Exhaust(ctx, cardModel, false, skipVisuals);
        await CardPileCmd.RemoveFromCombat(cardModel, true);
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
            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.Add(cardModel, pileType,
                    (pileType == PileType.Draw ? CardPilePosition.Random : CardPilePosition.Bottom)), 2f);
        }
        else
        {
            await Cmd.Wait(0.01f);
        }
    }

    public static async Task TimeForward(Creature creature, PlayerChoiceContext ctx, bool isIntended = true)
    {
        if (isIntended && creature.HasPower<EntanglementPower>())
        {
            await PowerCmd.Apply<DeviationPower>(ctx, creature, 1, creature, null);
            return;
        }
        
        if (!creature.HasPower<ForwardPower>())
        {
            if (creature.HasPower<BackwardPower>()) await PowerCmd.Remove<BackwardPower>(creature);
            await PowerCmd.Apply<ForwardPower>(ctx, creature, 1, creature, null);
            await PowerCmd.Apply<DeviationPower>(ctx, creature, 1, creature, null);
        }
        
        if (creature.GetPower<CrescentPower>() != null)
        {
            await PowerCmd.Remove<CrescentPower>(creature);
            await PowerCmd.Apply<PlenilunePower>(ctx, creature, 1, creature, null);
        } 
        else if (creature.GetPower<PlenilunePower>() != null)
        {
            await PowerCmd.Remove<PlenilunePower>(creature);
            await PowerCmd.Apply<DecrescentPower>(ctx, creature, 1, creature, null);
        }
        else
        {
            if (creature.GetPower<DecrescentPower>() != null) await PowerCmd.Remove<DecrescentPower>(creature);
            await PowerCmd.Apply<CrescentPower>(ctx, creature, 1, creature, null);
        }
    }

    public static async Task TimeBackward(Creature creature, PlayerChoiceContext ctx, bool isIntended = true)
    {
        if (isIntended && creature.HasPower<EntanglementPower>())
        {
            await PowerCmd.Apply<DeviationPower>(ctx, creature, 1, creature, null);
            return;
        }
        
        if (!creature.HasPower<BackwardPower>())
        {
            if (creature.HasPower<ForwardPower>()) await PowerCmd.Remove<ForwardPower>(creature);
            await PowerCmd.Apply<BackwardPower>(ctx, creature, 1, creature, null);
            await PowerCmd.Apply<DeviationPower>(ctx, creature, 1, creature, null);
        }
        
        if (creature.GetPower<DecrescentPower>() != null)
        {
            await PowerCmd.Remove<DecrescentPower>(creature);
            await PowerCmd.Apply<PlenilunePower>(ctx, creature, 1, creature, null);
        } 
        else if (creature.GetPower<PlenilunePower>() != null)
        {
            await PowerCmd.Remove<PlenilunePower>(creature);
            await PowerCmd.Apply<CrescentPower>(ctx, creature, 1, creature, null);
        }
        else
        {
            if (creature.GetPower<CrescentPower>() != null) await PowerCmd.Remove<CrescentPower>(creature);
            await PowerCmd.Apply<DecrescentPower>(ctx, creature, 1, creature, null);
        }
    }

    public static bool IsCrescent(Creature? creature)
    {
        if (creature == null) return false;
        return (creature.GetPower<CrescentPower>() != null) || (creature.GetPower<EntanglementPower>() != null);
    }
    public static bool IsPlenilune(Creature? creature)
    {
        if (creature == null) return false;
        return (creature.GetPower<PlenilunePower>() != null) || (creature.GetPower<EntanglementPower>() != null);
    }
    public static bool IsDecrescent(Creature? creature)
    {
        if (creature == null) return false;
        return (creature.GetPower<DecrescentPower>() != null) || (creature.GetPower<EntanglementPower>() != null);
    }
}