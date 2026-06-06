using BaseLib.Patches.Compatibility;   // CustomEnumAttribute
using BaseLib.Patches.Content;          // KeywordPropertiesAttribute, AutoKeywordPosition
using MegaCrit.Sts2.Core.Entities.Cards;

namespace SakikoMod.SakikoModCode.Character;

public class SakikoModKeywords
{
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None, true)]
    public static CardKeyword Generated = CardKeyword.None;
    
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.After, true)]
    public static CardKeyword ToBeDeleted = CardKeyword.None;
    
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None, true)]
    public static CardKeyword ToBeAdded = CardKeyword.None;
    
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None, true)]
    public static CardKeyword AsStrike = CardKeyword.None;
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None, true)]
    public static CardKeyword AsDefend = CardKeyword.None;
}