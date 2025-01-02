using System;
using BepInEx.Logging;

namespace ZetAspectsTranslator.Services;

internal static class Log
{
    private static ManualLogSource _logSource;
    private static bool _inited;

    internal static void Init(ManualLogSource logSource)
    {
        _logSource = logSource;
        _inited = true;
    }

    internal static void Debug(object data) => TryExecute(_logSource.LogDebug, data);
    internal static void Error(object data) => TryExecute(_logSource.LogError, data);
    internal static void Fatal(object data) => TryExecute(_logSource.LogFatal, data);
    internal static void Info(object data) => TryExecute(_logSource.LogInfo, data);
    internal static void Message(object data) => TryExecute(_logSource.LogMessage, data);
    internal static void Warning(object data) => TryExecute(_logSource.LogWarning, data);

    private static void TryExecute(Action<object> logMethod, object data)
    {
        if (_inited && _logSource != null) logMethod(data);
    }
}