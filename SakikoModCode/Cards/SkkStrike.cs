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
public class SkkStrike : SakikoCharacterBaseCard
{
//    public override string PortraitPath => "res://MzmChar/cards/strike.png";
    
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(6, ValueProp.Move),
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    private readonly HashSet<CardTag> _tags = new() { CardTag.Strike };
    protected override HashSet<CardTag> CanonicalTags => _tags;
    
    public SkkStrike() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }
    
    public override TargetType TargetType => TargetType.AnyEnemy;
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);              // 6 → 9
    }
    
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, play).Targeting(play.Target).Execute(ctx);
        }
    }
}