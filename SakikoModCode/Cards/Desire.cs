using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Desire : SakikoCharacterBaseCard
{
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new BlockVar(8, ValueProp.Move),
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Exhaust, SakikoModKeywords.AsDefend };
    public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;
    
    private readonly HashSet<CardTag> _tags = new() { CardTag.Defend };
    protected override HashSet<CardTag> CanonicalTags => _tags;

    private readonly HashSet<SakikoCardTag> _sakikoTags = new() { SakikoCardTag.Mang };
    protected override HashSet<SakikoCardTag> CanonicalSakikoTags => _sakikoTags;

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4);               // 8 → 12
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature,
            DynamicVars.Block.BaseValue, ValueProp.Move, play, false);
    }
    
    public Desire() : base(1, CardType.Skill, CardRarity.Common, TargetType.None) { }
}