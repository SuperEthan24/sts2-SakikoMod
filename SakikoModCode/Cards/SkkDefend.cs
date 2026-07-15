using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
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
public class SkkDefend : SakikoCharacterBaseCard
{
    // 初始卡，不应被印牌/变化牌随机产生
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;

    private readonly List<DynamicVar> _vars = new()
    {
        new BlockVar(5, ValueProp.Move),                    // Block
        new BlockVar("ExtraBlock", 3, ValueProp.Move)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    private readonly HashSet<CardTag> _tags = new() { CardTag.Defend };
    protected override HashSet<CardTag> CanonicalTags => _tags;
    
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);               // 5 → 8
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature,
            DynamicVars.Block.BaseValue, ValueProp.Move, play, false);
    }

    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        DynamicVars.Block.BaseValue += DynamicVars["ExtraBlock"].BaseValue;
    }

    public SkkDefend() : base(1, CardType.Skill, CardRarity.Basic, TargetType.None) { }
}