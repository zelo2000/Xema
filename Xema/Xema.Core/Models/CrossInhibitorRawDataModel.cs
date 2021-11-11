using System.Collections.Generic;

namespace Xema.Core.Models
{
    public class CrossInhibitorRawDataModel
    {
        public CrossInhibitorRawDataModel()
        {
            AntigenLabels = new List<string>();
            MarkedAntigenLabels = new List<string>();
            BlankIndexes = new List<double>();
            CrossInhibitionIndexes = new List<List<IndexCell>>();
        }

        public List<string> AntigenLabels { get; set; }
        public List<string> MarkedAntigenLabels { get; set; }

        public List<double> BlankIndexes { get; set; }

        public List<List<IndexCell>> CrossInhibitionIndexes { get; set; }
    }
}
