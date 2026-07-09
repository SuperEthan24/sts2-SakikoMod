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

[Pool(typeof(SakikoCharacterCardPool))]
public class WannaBeHuman : SakikoCharacterBaseCard
{
    private readonly List<DynamicVar> _vars = new()
    {
        new BlockVar(7, ValueProp.Move)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

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
        await CreatureCmd.GainBlock(Owner.Creature,
            DynamicVars.Block.BaseValue, ValueProp.Move, play, false);
        await SakikoModCmd.InGameAdd(base.Owner.Creature, ctx, base.Owner.Creature.CombatState.CreateCard<Nihil>(base.Owner),
            PileType.Draw);
    }

    public WannaBeHuman() : base(1, CardType.Skill, CardRarity.Common, TargetType.None) {}
}