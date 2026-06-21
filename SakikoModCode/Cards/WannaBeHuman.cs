using System.ComponentModel;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class WannaBeHuman : SakikoModBaseCard
{
    private readonly List<DynamicVar> _vars = new()
    {
        new BlockVar(7, ValueProp.Move),
        new BackwardVar(1)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    private readonly HashSet<CardKeyword> _keywords = new() { SakikoModKeywords.BackwardKeyword };
    public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromCard<Nihil>();
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        for (int i = 0; i < DynamicVars["Backward"].BaseValue; i++)
        {
            await SakikoModCmd.TimeBackward(base.Owner.Creature, ctx);
        }
        await CreatureCmd.GainBlock(Owner.Creature,
            DynamicVars.Block.BaseValue, ValueProp.Move, play, false);
        await SakikoModCmd.InGameAdd(base.Owner.Creature, ctx, base.Owner.Creature.CombatState.CreateCard<Nihil>(base.Owner),
            PileType.Draw);
    }

    public WannaBeHuman() : base(1, CardType.Skill, CardRarity.Common, TargetType.None) {}
}