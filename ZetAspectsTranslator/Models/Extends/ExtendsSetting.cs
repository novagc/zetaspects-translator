using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ZetAspectsTranslator.Models.Extends;

public class ExtendsSetting
{
    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(StringEnumConverter), typeof(CamelCaseNamingStrategy))]
    public ExtendsType Type { get; set; } = ExtendsType.Tokens;
}