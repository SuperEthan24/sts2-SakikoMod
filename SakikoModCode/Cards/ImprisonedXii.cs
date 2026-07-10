using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class ImprisonedXii : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new BlockVar(30, ValueProp.Move),
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;

	protected override void OnUpgrade()
	{
		DynamicVars.Block.UpgradeValueBy(10);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
	{
		await CreatureCmd.GainBlock(base.Owner.Creature, DynamicVars.Block, play);
		PlayerCmd.EndTurn(base.Owner, false);
	}

	public ImprisonedXii() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) {}
}