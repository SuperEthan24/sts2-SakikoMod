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
public class ForgetEverything : SakikoModBaseCard
{
    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(8, ValueProp.Move),
        new ForwardVar(1)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    private readonly HashSet<CardKeyword> _keywords = new() { SakikoModKeywords.ForwardKeyword };
    public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromCard<Desire>();
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        for (int i = 0; i < DynamicVars["Forward"].BaseValue; i++)
        {
            await SakikoModCmd.TimeForward(base.Owner.Creature, ctx);
        }
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(play.Target).Execute(ctx);
        }

        await SakikoModCmd.InGameAdd(base.Owner.Creature, ctx,
            base.Owner.Creature.CombatState.CreateCard<Desire>(base.Owner), PileType.Draw);
    }
    
    public ForgetEverything() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
}