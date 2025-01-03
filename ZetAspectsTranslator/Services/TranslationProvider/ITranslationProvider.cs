using System.Collections.Generic;

namespace ZetAspectsTranslator.Services.TranslationProvider;

public interface ITranslationProvider
{
    string Language { get; }
    Dictionary<string, string> Tokens { get; }
}