using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using SakikoMod.SakikoModCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using SakikoMod.SakikoModCode.Cards;
using MegaCrit.Sts2.Core.Helpers;
using SakikoMod.SakikoModCode.Relics;

namespace SakikoMod.SakikoModCode.Character;

// ReSharper disable once ClassNeverInstantiated.Global
public class SakikoCharacter : CustomCharacterModel
{
    /// <summary>所有借用资源都从这个角色派生。改成 "necrobinder" / "silent" 等一键换占位风格。</summary>
    private const string BorrowFrom = "ironclad";
    
    // ===== 必填：核心数值 =====
    // 主题色：深 sage 绿（呼应小睦的灰绿色头发，比头发再深一点）
    private static readonly Color ThemeColor = new(0.30f, 0.55f, 0.40f);

    public override Color NameColor => ThemeColor;
    public override Color MapDrawingColor => ThemeColor;   // 地图上画路径的颜色
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 79;
    public override int StartingGold => 100;

    // ===== [OURS] 我们自己出的视觉资源 ===== //
    // 选角界面 —— 用户最直接看到我们角色的地方
    public override string? CustomCharacterSelectIconPath => "res://SakikoMod/images/characters/select.png";
    public override string? CustomCharacterSelectLockedIconPath => "res://SakikoMod/images/characters/select.png";
    public override string? CustomCharacterSelectBg => "res://SakikoMod/scenes/char_select_bg.tscn";
    // 顶部信息栏头像
    // CustomIconTexturePath 被 vanilla 多处直接塞进 TextureRect.Texture（history page / continue
    // run info / 多人 lobby / 对话框 speaker 等 7 处），节点 stretch_mode 由 vanilla 各 .tscn 决定。
    // beta v0.105 改了 NRunHistoryPlayerIcon._icon 的 stretch 配置 → 大图（256×256）爆开。
    // 走 vanilla 同尺寸范围（≤128）的 button_small.png；顶部 HUD 仍用 character_icon.tscn 引高清 button.png。
    public override string? CustomIconTexturePath => "res://SakikoMod/images/characters/button_small.png";
    public override string? CustomIconOutlineTexturePath => "res://SakikoMod/images/characters/button.png";
    
    // ===== 战斗中角色视觉 —— 用我们的 visuals.tscn (含 BaseLib 要求的 7 个子节点) =====
    // 想换成 Spine 动画：把 visuals.tscn 里的 Visuals 节点从 Sprite2D 改成 SpineSprite + skeleton_data_res
    public override string? CustomVisualPath => "res://SakikoMod/scenes/visuals.tscn";
    
    // ===== [BORROW: ironclad] 动画 / 商店 / 休息处 / SFX ===== //
    // 卡片拖尾 vfx
    public override string? CustomTrailPath => SceneHelper.GetScenePath("vfx/card_trail_" + BorrowFrom);
    // 顶部信息栏的整个头像 Control —— 用我们的 character_icon.tscn（TextureRect 包 button.png）
    public override string? CustomIconPath => "res://SakikoMod/scenes/character_icon.tscn";
    
    // 能量计数器：路径借 Ironclad 的场景结构，但用 CustomEnergyCounter struct 把图层换成我们自己的 energy_big.png
    // BaseLib 的 EnergyCounterPatch 会拦截 NEnergyCounter.Create 用我们的 pathFunc 替换图层
    public override string? CustomEnergyCounterPath => SceneHelper.GetScenePath("combat/energy_counters/" + BorrowFrom + "_energy_counter");
    public override CustomEnergyCounter? CustomEnergyCounter => new(
        pathFunc: layer => "res://SakikoMod/images/characters/energy_big.png",
        outlineColor: ThemeColor,
        burstColor: Colors.White);
    // 休息处坐姿 -- 用我们自己的 rest_site.tscn (Sprite2D 显示 rest_site_portrait.png)
    public override string? CustomRestSiteAnimPath => "res://SakikoMod/scenes/rest_site.tscn";
    // 商店站姿 -- 用我们自己的 merchant.tscn (Sprite2D 显示 merchant_portrait.png)
    public override string? CustomMerchantAnimPath => "res://SakikoMod/scenes/merchant.tscn";
    
    // 地图上的玩家标记
    public override string? CustomMapMarkerPath => ImageHelper.GetImagePath("packed/map/icons/map_marker_" + BorrowFrom + ".png");
    // 选角转场材质 —— 自做的 shader 是这次白屏的最可疑嫌犯，借 Ironclad 的稳
    public override string? CustomCharacterSelectTransitionPath => "res://materials/transitions/" + BorrowFrom + "_transition_mat.tres";
    // 多人模式手势贴图
    /*
    public override string? CustomArmPointingTexturePath => "res://MzmChar/characters/hand_point.png";
    public override string? CustomArmRockTexturePath => "res://MzmChar/characters/hand_rock.png";
    public override string? CustomArmPaperTexturePath => "res://MzmChar/characters/hand_paper.png";
    public override string? CustomArmScissorsTexturePath => "res://MzmChar/characters/hand_scissors.png";
    */

    // SFX —— 这些是 CharacterModel 直接虚属性（不带 "Custom" 前缀），返回 FMOD event 路径
    public override string CharacterSelectSfx => $"event:/sfx/characters/{BorrowFrom}/{BorrowFrom}_select";
    public override string CharacterTransitionSfx => $"event:/sfx/ui/wipe_{BorrowFrom}";
    public override string? CustomAttackSfx => $"event:/sfx/characters/{BorrowFrom}/{BorrowFrom}_attack";
    public override string? CustomCastSfx => $"event:/sfx/characters/{BorrowFrom}/{BorrowFrom}_cast";
    public override string? CustomDeathSfx => $"event:/sfx/characters/{BorrowFrom}/{BorrowFrom}_die";

    public override List<string> GetArchitectAttackVfx() => new()
    {
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter",
    };
    public override IEnumerable<CardModel> StartingDeck =>
        Enumerable.Repeat<CardModel>(ModelDb.Card<SkkStrike>(), 5)
            .Concat(Enumerable.Repeat<CardModel>(ModelDb.Card<SkkDefend>(), 5))
            .Append<CardModel>(ModelDb.Card<Oblivionis>())
            .Append<CardModel>(ModelDb.Card<GoldAttack>())
            .Append<CardModel>(ModelDb.Card<DeliverNewspaper>());

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<WornRibbonRelic>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<SakikoCharacterCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<SakikoCharacterRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<SakikoCharacterPotionPool>();
}