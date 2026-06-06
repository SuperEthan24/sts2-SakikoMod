using BaseLib.Abstracts;
using SakikoMod.SakikoModCode.Extensions;
using Godot;

namespace SakikoMod.SakikoModCode.Character;

public class SakikoModRelicPool : CustomRelicPoolModel
{
    public override bool IsShared => false;
    public override string? BigEnergyIconPath  => "res://SakikoMod/images/characters/energy_big.png";
    public override string? TextEnergyIconPath => "res://SakikoMod/images/characters/energy_text.png";
}