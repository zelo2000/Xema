using Newtonsoft.Json;
using System.Collections.Generic;
using Xema.Core.Enums;

namespace Xema.ClusterAPI.Models
{
    public class ClusterResponce
    {
        [JsonProperty("clusters")]
        public Dictionary<string, int> Clusters { get; set; }

        [JsonProperty("initial")]
        public Dictionary<string, Dictionary<string, double>> InitialValue { get; set; }

        [JsonProperty("index")]
        public Dictionary<string, Dictionary<string, double>> Indexes { get; set; }

        [JsonProperty("color")]
        public Dictionary<string, Dictionary<string, InhibitionColors>> Colors { get; set; }
    }
}
