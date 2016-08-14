using System;
using Newtonsoft.Json;

namespace Mrtn.Data
{
    public sealed class DataPackage
    {
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("bld")]
        public string Building { get; set; }

        [JsonProperty("src")]
        public string Source { get; set; }

        [JsonProperty("flats")]
        public Flat[] Flats { get; set; }

        [JsonProperty("aggrs")]
        public AggregatedInfo[] AggregatedInfos { get; set; }
    }
}