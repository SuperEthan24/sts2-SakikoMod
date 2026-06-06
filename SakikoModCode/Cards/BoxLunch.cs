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
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoModCardPool))]
public class BoxLunch : SakikoModBaseCard
{
    private readonly List<DynamicVar> _vars = new()
    {
        new DynamicVar("Heal", 7m),
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
    private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Ethereal };
    public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;
    
    protected override void OnUpgrade()
    {
        DynamicVars["Heal"].UpgradeValueBy(3);     // 7 → 10
    }
    
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<LunchPower>(ctx, base.Owner.Creature, amount:base.DynamicVars["Heal"].BaseValue, base.Owner.Creature, this);
    }

    public BoxLunch() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.None) { }
}