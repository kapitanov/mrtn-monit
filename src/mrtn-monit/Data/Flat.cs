using Newtonsoft.Json;

namespace Mrtn.Data
{
    public sealed class Flat
    {
        [JsonProperty("building")]
        public string Building { get; set; }

        [JsonProperty("block")]
        public int Block { get; set; }

        [JsonProperty("floor")]
        public int Floor { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("type")]
        public FlatType Type { get; set; }

        [JsonProperty("square")]
        public float Square { get; set; }

        [JsonProperty("balcony")]
        public bool HasBalcony { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("url")]
        public string PlanUrl { get; set; }
    }
}
