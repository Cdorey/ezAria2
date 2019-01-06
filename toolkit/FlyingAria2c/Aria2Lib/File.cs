using Newtonsoft.Json;

namespace FlyingAria2c.Aria2Lib
{
    struct File
    {
        [JsonProperty(PropertyName = "index")]
        public string Index { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "length")]
        public string Length { get; set; }
    }
}
