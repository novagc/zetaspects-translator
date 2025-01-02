using System;

namespace ZetAspectsTranslator.Models.Exceptions;

public class CyclicalDependenceException(string path) : Exception
{
    public string Path { get; } = path;
    public override string Message =>
        $"A cyclical relationship has been discovered. The file \"{Path}\"it has already been processed.";
}