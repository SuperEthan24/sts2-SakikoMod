using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class GatherInformation : SakikoCharacterBaseCard
{
    protected override bool ShouldGlowRedInternal => !IsPlayable;
    protected override bool IsPlayable => base.Owner.Gold >= DynamicVars["GoldCost"].BaseValue;

    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(9, ValueProp.Move),
        new GoldCostVar(10),
        new CardsVar(1)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Target != null)
        {
            await PlayerCmd.LoseGold(DynamicVars["GoldCost"].BaseValue, base.Owner);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).Execute(ctx);
            await CardPileCmd.Draw(ctx, DynamicVars.Cards.BaseValue, base.Owner);
        }
    }

    public GatherInformation() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) {}
}