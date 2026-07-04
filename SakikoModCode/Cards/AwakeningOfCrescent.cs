using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class AwakeningOfCrescent : SakikoCharacterBaseCard
{
    protected override bool ShouldGlowGoldInternal => SakikoModCmd.IsCrescent(base.Owner.Creature);

    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(4, ValueProp.Move),
        new DynamicVar("AttackTimes", 3),
        new DynamicVar("ExtraAttack", 2)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromPower<CrescentPower>();
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        int hits = (int)DynamicVars["AttackTimes"].BaseValue;
        if (SakikoModCmd.IsCrescent(base.Owner.Creature)) hits += (int)DynamicVars["ExtraAttack"].BaseValue;
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
                .WithHitCount(hits).Execute(ctx);
        }
    }

    public AwakeningOfCrescent() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) {}
}