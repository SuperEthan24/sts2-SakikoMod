using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class ForgetEverything : SakikoCharacterBaseCard
{
    private readonly List<DynamicVar> _vars = new()
    {
        new DamageVar(5, ValueProp.Move)
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return HoverTipFactory.FromKeyword(SakikoModKeywords.Deletion);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        IEnumerable<CardModel>? cards = PileType.Draw.GetPile(base.Owner).Cards
            .Where((CardModel c) => c.Type is CardType.Curse);
        decimal cardCount = cards.Count();
        await SakikoModCmd.InGameDelete(base.Owner.Creature, ctx, cards);
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).WithHitCount((int)cardCount + 1)
                .Targeting(play.Target).Execute(ctx);
        }
    }
    
    public ForgetEverything() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
}