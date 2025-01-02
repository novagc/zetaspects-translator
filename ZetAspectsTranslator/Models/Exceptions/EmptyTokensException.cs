using System;

namespace ZetAspectsTranslator.Models.Exceptions;

public class EmptyTokensException: Exception
{
    private readonly string _language;
    
    public EmptyTokensException(string language)
    {
        _language = language;
    }
    public override string Message => $"The {_language} translation does not contain tokens";
}