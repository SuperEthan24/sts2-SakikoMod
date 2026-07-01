using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
// CustomEnumAttribute
// KeywordPropertiesAttribute, AutoKeywordPosition

namespace SakikoMod.SakikoModCode;

public static class SakikoModKeywords
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
    public static CardKeyword Addition = CardKeyword.None;
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.After, true)]
    public static CardKeyword Deletion = CardKeyword.None;
    
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None, true)]
    public static CardKeyword AsStrike = CardKeyword.None;
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None, true)]
    public static CardKeyword AsDefend = CardKeyword.None;
    
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None, true)]
    public static CardKeyword ForwardKeyword = CardKeyword.None;
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None, true)]
    public static CardKeyword BackwardKeyword = CardKeyword.None;
}