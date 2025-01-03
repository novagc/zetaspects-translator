using System;
using System.Collections.Generic;
using System.Linq;
using ZetAspectsTranslator.Models.Exceptions;
using ZetAspectsTranslator.Services;
using ZetAspectsTranslator.Services.TranslationProvider;

namespace ZetAspectsTranslator.Controllers;

public class TranslationController
{
    public void RegisterTranslations(IEnumerable<ITranslationProvider> translations)
    {
        foreach (var translation in translations)
            RegisterTranslation(translation);
    }
    
    public void RegisterTranslation(ITranslationProvider translation)
    {
        try
        {
            RegisterTokens(translation.Language, translation.Tokens);
         
            Log.Info($"Registered {translation.Language} translation ({translation.Tokens.Count} tokens)");
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }

    private void RegisterTokens(string language, Dictionary<string, string> tokens)
    {
        if (!tokens.Any()) throw new EmptyTokensException(language);
        
        var originalTargetLanguage = TPDespair.ZetAspects.Language.targetLanguage;
        TPDespair.ZetAspects.Language.targetLanguage = language;

        foreach (var token in tokens)
        {
            TPDespair.ZetAspects.Language.RegisterFragment(token.Key, token.Value);
        }
        
        TPDespair.ZetAspects.Language.targetLanguage = originalTargetLanguage;
    }
}

