using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Oblivionis : SakikoCharacterBaseCard
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
        await PowerCmd.Apply<SakikoOblivionPower>(ctx, base.Owner.Creature, 1m, base.Owner.Creature, this);
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, play).Targeting(play.Target).Execute(ctx);
        }
    }
    
    public Oblivionis() : base(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }
}