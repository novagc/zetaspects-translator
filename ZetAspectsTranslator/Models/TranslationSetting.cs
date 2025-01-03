using System.Collections.Generic;
using Newtonsoft.Json;
using ZetAspectsTranslator.Models.Extends;

namespace ZetAspectsTranslator.Models;

public class TranslationSetting
{
    [JsonProperty("language")]
    public string Language { get; set; }

    [JsonProperty("tokens", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, string> Tokens { get; set; } = [];

    [JsonProperty("extends", NullValueHandling = NullValueHandling.Ignore)]
    public ExtendsSetting[] Extends { get; set; } = [];
}