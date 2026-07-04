using MegaCrit.Sts2.Core.Localization;

namespace SakikoMod.SakikoModCode.Cards;

using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

[Pool(typeof(SakikoCharacterCardPool))]
public class Nihil : SakikoCharacterBaseCard
{
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(10, ValueProp.Move),
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Exhaust, SakikoModKeywords.AsStrike };
    public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;
    
    private readonly HashSet<CardTag> _tags = new() { CardTag.Strike };
    protected override HashSet<CardTag> CanonicalTags => _tags;
    
    private readonly HashSet<SakikoCardTag> _sakikoTags = new() { SakikoCardTag.Mang };
    protected override HashSet<SakikoCardTag> CanonicalSakikoTags => _sakikoTags;

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);               // 10 → 14
    }
    
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, play).Targeting(play.Target).Execute(ctx);
        }
    }
    
    public Nihil() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
}