using Newtonsoft.Json;

namespace Experimental.Common;

[JsonObject]
public class Model
{
    [JsonProperty("id")]
    public required Guid Id { get; init; }

    [JsonProperty("name")]
    public required string Name { get; init; }
}
