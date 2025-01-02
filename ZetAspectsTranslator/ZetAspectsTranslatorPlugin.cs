using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using ZetAspectsTranslator.Controllers;
using ZetAspectsTranslator.Services;
using ZetAspectsTranslator.Services.TranslationProvider;

namespace ZetAspectsTranslator;

// ReSharper disable MemberCanBePrivate.Global

[BepInDependency("com.TPDespair.ZetAspects")]
[BepInPlugin(Guid, Name, Version)]
public class ZetAspectsTranslatorPlugin : BaseUnityPlugin
{
    public const string Guid = Author + "." + Name;
    public const string Author = "NovaGC";
    public const string Name = "ZetAspectsTranslator";
    public const string Version = "1.0.0";

    private readonly string[] _schemaFiles =
    [
        "settings.schema.json",
        "tokens.schema.json",
    ];
    
    public void Awake()
    {
        Log.Init(Logger);

        var pathInPlugin = Path.Combine(
            Assembly.GetAssembly(GetType()).Location,
            "translations"
        );

        var pathInGame = Path.Combine(
            Paths.GameRootPath,
            "Risk of Rain 2_Data",
            "StreamingAssets",
            "LanguageOverrides",
            "ZetAspects"
        );
        
        Log.Info($"\n{pathInPlugin}\n{pathInGame}");
        
        InitFoldersAndSchemas(pathInPlugin, pathInGame);
        
        var translationController = new TranslationController();

        var pathsWithTranslations = new []
        {
            pathInPlugin,
            pathInGame,
        };

        foreach (var path in pathsWithTranslations)
        {
            var settings = Directory
                .GetFiles(path, "*.json")
                .Select(x => new FileInfo(x))
                .Where(x => !_schemaFiles.Contains(x.Name))
                .OrderByDescending(x => x.Name)
                .Select(x => new TranslationFileProvider(path, x.FullName))
                .ToList();
            
            translationController.RegisterTranslations(settings);
        }
    }

    private void InitFoldersAndSchemas(string pathInPlugin, string pathInGame)
    {
        var requiredFolders = new[]
        {
            pathInPlugin,
            pathInGame,
            Path.Combine(pathInPlugin, "tokens"),
            Path.Combine(pathInGame, "tokens"),
        };

        foreach (var path in requiredFolders.Where(x => !Directory.Exists(x)))
        {
            Directory.CreateDirectory(pathInGame);
        }

        var schemasToCopy = _schemaFiles
            .Select(x => new FileInfo(Path.Combine(pathInPlugin, x)))
            .Where(x => x.Exists);

        foreach (var schemaToCopy in schemasToCopy)
        {
            schemaToCopy.CopyTo(Path.Combine(pathInGame, schemaToCopy.Name), true);
        }
    }
}

