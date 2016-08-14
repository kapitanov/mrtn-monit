using Newtonsoft.Json;

namespace Mrtn.Data
{
    public sealed class AggregatedInfo : AggregatedInfoKey
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("prices")]
        public PriceRange Prices { get; set; } = new PriceRange();

        [JsonProperty("pricesPerSquare")]
        public PriceRange PricesPerSquare { get; set; } = new PriceRange();
    }
}