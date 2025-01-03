using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZetAspectsTranslator.Models;
using ZetAspectsTranslator.Models.Exceptions;
using ZetAspectsTranslator.Models.Extends;

namespace ZetAspectsTranslator.Services.TranslationProvider;

public class TranslationFileProvider: ITranslationProvider
{
    public string Language { get; private set; }
    public Dictionary<string, string> Tokens { get; }

    private readonly string _baseTranslationsPath;
    private readonly HashSet<string> _processedFiles;
    
    public TranslationFileProvider(string baseTranslationsPath, string translationFileName)
    {
        _baseTranslationsPath = baseTranslationsPath;
        _processedFiles = new();

        Language = null;
        Tokens = new Dictionary<string, string>();
        
        LoadTranslations(translationFileName);
    }

    private void LoadTranslations(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException(path);

        var translationStack = new Stack<ExtendsSetting>(
            new []{
                new ExtendsSetting
                {
                    Path = path,
                    Type = ExtendsType.Setting
                } 
            }
        );

        while (translationStack.Any())
        {
            try
            {
                var currentSetting = translationStack.Pop();
                currentSetting.Path = GetFullPath(currentSetting.Path);
                
                var newExtendsSettings = ProcessSetting(currentSetting);

                foreach (var extendsSetting in newExtendsSettings.Reverse())
                {
                    translationStack.Push(extendsSetting);
                }

                _processedFiles.Add(currentSetting.Path);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }

    private ExtendsSetting[] ProcessSetting(ExtendsSetting setting)
    {
        if (!File.Exists(setting.Path)) throw new FileNotFoundException(setting.Path);
        if (_processedFiles.Contains(setting.Path)) throw new CyclicalDependenceException(setting.Path);
        
        if (setting.Type == ExtendsType.Tokens)
        {
            var tokens = ParseJsonFile<Dictionary<string, string>>(setting.Path);
            MergeTokens(tokens);
            return [];
        }

        var translationSetting = ParseJsonFile<TranslationSetting>(setting.Path);
        Language = String.IsNullOrEmpty(Language) ? translationSetting.Language : Language;
        MergeTokens(translationSetting.Tokens);

        return translationSetting.Extends;
    }
    
    private T ParseJsonFile<T>(string path)
    {
        using var translationSettingFileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var translationSettingReader = new StreamReader(translationSettingFileStream);
        using var translationSettingJsonReader = new JsonTextReader(translationSettingReader);

        var serializer = new JsonSerializer();
        return serializer.Deserialize<T>(translationSettingJsonReader);
    }
    
    private void MergeTokens(Dictionary<string, string> tokens)
    {
        if(!tokens.Any()) return;

        var tokensToMerge = tokens
            .Where(token => token.Key != "$schema")
            .Where(token => !Tokens.ContainsKey(token.Key));
        
        foreach (var token in tokensToMerge)
        {
            Tokens.Add(token.Key, token.Value);
        }
    }

    private string GetFullPath(string path) => Path.IsPathFullyQualified(path)
        ? path
        : Path.Combine(_baseTranslationsPath, path);
}