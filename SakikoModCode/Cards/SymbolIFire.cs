using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class SymbolIFire : SakikoCharacterBaseCard
{
    private readonly List<DynamicVar> _vars = new()
    {
        new PowerVar<RitualPower>(3)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    protected override void OnUpgrade()
    {
        DynamicVars["RitualPower"].UpgradeValueBy(1);
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromPower<RitualPower>();
            yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Deletion);
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        CardModel? cardModel = (await SakikoModCmd.CardSelectFromHandForDelete(ctx, base.Owner,
            new CardSelectorPrefs(new LocString("gameplay_ui", "SAKIKOMOD-SELECT_CARD_DELETE"),
                1), null, this)).FirstOrDefault();
        if (cardModel != null)
        {
            await SakikoModCmd.InGameDelete(base.Owner.Creature, ctx, cardModel);
            await PowerCmd.Apply<RitualPower>(ctx, base.Owner.Creature, DynamicVars["RitualPower"].BaseValue,
                base.Owner.Creature, this);
        }
    }

    public SymbolIFire() : base(0, CardType.Skill, CardRarity.Rare, TargetType.None) { }
}