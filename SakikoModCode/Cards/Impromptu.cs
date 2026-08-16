using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using SakikoMod.SakikoModCode.Character;

namespace SakikoMod.SakikoModCode.Cards;

[Pool(typeof(SakikoCharacterCardPool))]
public class Impromptu : SakikoCharacterBaseCard
{
	private readonly List<DynamicVar> _vars = new()
	{
		new EnergyVar(1),
		new CardsVar(2)
	};
	protected override IEnumerable<DynamicVar> CanonicalVars => _vars;
	
	private readonly HashSet<CardKeyword> _keywords = new() { CardKeyword.Retain, CardKeyword.Exhaust };
	public override IEnumerable<CardKeyword> CanonicalKeywords => _keywords;

	protected override void OnUpgrade()
	{
		DynamicVars.Energy.UpgradeValueBy(1);
	}

	protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay _)
	{
		await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, base.Owner);
		await CardPileCmd.Draw(ctx, base.Owner);
	}

	public override async Task BeforeHandDraw(Player player, PlayerChoiceContext ctx, ICombatState combatState)
	{
		if (player != base.Owner) return;
		if (CombatManager.Instance.History.CardPlaysFinished.Any((CardPlayFinishedEntry e) =>
			    e.HappenedLastPlayerTurn(base.Owner) && e.CardPlay.Card == this))
		{
			CardPile? pile = base.Pile;
			if (pile == null || pile.Type != PileType.Draw)
			{
				await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Random);
			}
		}
	}

	public Impromptu() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {}
}