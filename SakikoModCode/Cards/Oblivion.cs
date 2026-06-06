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
using OblivionPower = SakikoMod.SakikoModCode.Powers.OblivionPower;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class Oblivion : SakikoModBaseCard
{
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;
    
    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(10, ValueProp.Move),
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Exhaust };
    public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;
    
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);   // 2 → 1
    }
    
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<OblivionPower>(ctx, base.Owner.Creature, 1m, base.Owner.Creature, this);
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(play.Target).Execute(ctx);
        }
    }
    
    public Oblivion() : base(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }
}