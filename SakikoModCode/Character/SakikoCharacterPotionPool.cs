using BaseLib.Abstracts;
using SakikoMod.SakikoModCode.Extensions;
using Godot;

namespace SakikoMod.SakikoModCode.Character;

public class SakikoCharacterPotionPool : CustomPotionPoolModel
{
//    public override Color LabOutlineColor => SakikoMod.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}