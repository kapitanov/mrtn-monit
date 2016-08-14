using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mrtn.Data
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FlatType
    {
        [EnumMember(Value = "CT")]
        Studio,

        [EnumMember(Value = "1")]
        Flat1,

        [EnumMember(Value = "2")]
        Flat2,

        [EnumMember(Value = "2E")]
        Flat2E,

        [EnumMember(Value = "3")]
        Flat3,

        [EnumMember(Value = "3E")]
        Flat3E
    }
}