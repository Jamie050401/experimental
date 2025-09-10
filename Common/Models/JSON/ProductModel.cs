using Newtonsoft.Json;

namespace Experimental.Common.Models.JSON;

[JsonObject]
public class ProductModel
{
    [JsonProperty("id")]
    public required Guid Id { get; init; }

    [JsonProperty("name")]
    public required string Name { get; init; }
}
