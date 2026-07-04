using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Extensions;
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
public class GoldAttack : SakikoCharacterBaseCard
{
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;

    protected override bool ShouldGlowRedInternal => !IsPlayable;
    protected override bool IsPlayable => base.Owner.Gold >= DynamicVars["GoldCost"].BaseValue;

    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(8, ValueProp.Move),
        new GoldCostVar(10),
        new PowerVar<VulnerablePower>(2),
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromPower<VulnerablePower>();
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Target != null)
        {
            await PlayerCmd.LoseGold(DynamicVars["GoldCost"].BaseValue, base.Owner);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<VulnerablePower>(ctx, play.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
        }
    }

    public GoldAttack() : base(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }
}