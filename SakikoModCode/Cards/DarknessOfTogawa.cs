using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SakikoMod.SakikoModCode.Character;
using SakikoMod.SakikoModCode.Powers;

namespace SakikoMod.SakikoModCode.Cards;

[Pool((typeof(SakikoCharacterCardPool)))]
public class DarknessOfTogawa : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new PowerVar<DarknessOfTogawaPower>(2)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
    
	private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Ethereal };
	public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

	protected override void OnUpgrade()
	{
		CardCmd.RemoveKeyword(this, CardKeyword.Ethereal);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay _)
	{
		await PowerCmd.Apply<DarknessOfTogawaPower>(ctx, base.Owner.Creature,
			DynamicVars.Power<DarknessOfTogawaPower>().BaseValue, base.Owner.Creature, this);
	}

	public DarknessOfTogawa() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) {}
}