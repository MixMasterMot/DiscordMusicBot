using Newtonsoft.Json;

namespace motBot2.Services.YouTube
{
    public class StreamFormatMetadata
    {
        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "acodec")]
        public string Codec { get; set; }
    }
}
