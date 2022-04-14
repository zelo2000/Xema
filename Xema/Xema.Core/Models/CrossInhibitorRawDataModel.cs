using System.Collections.Generic;

namespace Xema.Core.Models
{
    public class CrossInhibitorRawDataModel
    {
        public List<List<string>> AntigenLabels { get; set; }

        public List<string> MarkedAntigenLabels { get; set; }

        public List<List<List<IndexCell>>> CrossInhibitionIndexes { get; set; }

        public Dictionary<int, List<string>> Clusters { get; set; }
    }
}
