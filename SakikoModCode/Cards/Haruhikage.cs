using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class Haruhikage : SakikoModBaseCard, ITomeCard
{
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new DynamicVar("Haruhikage", 2m),
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Ethereal };
    public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromPower<HaruhikagePower>();
            yield return HoverTipFactory.FromPower<SakikoLightPower>();
            yield return HoverTipFactory.FromPower<SakikoDarkPower>();
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<HaruhikagePower>(choiceContext, base.Owner.Creature,
            amount: base.DynamicVars["Haruhikage"].BaseValue, base.Owner.Creature, this);
    }

    public Haruhikage() : base(3, CardType.Power, CardRarity.Ancient, TargetType.Self) { }
}