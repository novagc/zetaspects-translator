using System;
using System.Linq;
using BepInEx.Logging;

namespace ZetAspectsTranslator.Services;

internal static class Log
{
    private static ManualLogSource _logSource;
    private static Action<object>[] _logMethods = Enumerable.Repeat((object _) => { }, 6).ToArray();
    
    internal static void Init(ManualLogSource logSource)
    {
        _logSource = logSource;
        _logMethods =
        [
            _logSource.LogDebug,
            _logSource.LogError,
            _logSource.LogFatal,
            _logSource.LogInfo,
            _logSource.LogMessage,
            _logSource.LogWarning,
        ];
    }

    internal static void Debug(object data) => _logMethods[0](data);
    internal static void Error(object data) => _logMethods[1](data);
    internal static void Fatal(object data) => _logMethods[2](data);
    internal static void Info(object data) => _logMethods[3](data);
    internal static void Message(object data) => _logMethods[4](data);
    internal static void Warning(object data) => _logMethods[5](data);
}