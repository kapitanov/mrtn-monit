using Newtonsoft.Json;

namespace Mrtn.Data
{
    public class AggregatedInfoKey
    {
        [JsonProperty("type")]
        public FlatType Type { get; set; }

        [JsonProperty("square")]
        public float Square { get; set; }

        [JsonProperty("balcony")]
        public bool HasBalcony { get; set; }

        protected bool Equals(AggregatedInfoKey other)
        {
            return Type == other.Type && Square.Equals(other.Square) && HasBalcony == other.HasBalcony;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as AggregatedInfoKey;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;
                hashCode = (hashCode*397) ^ Square.GetHashCode();
                hashCode = (hashCode*397) ^ HasBalcony.GetHashCode();
                return hashCode;
            }
        }
    }
}