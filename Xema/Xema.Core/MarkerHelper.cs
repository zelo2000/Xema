using System.Collections.Generic;
using System.Linq;

namespace Xema.Core
{
    public class MarkerHelper
    {
        public static int Mark(double cellValue, double maxValue, List<double> marks)
        {
            marks = marks.OrderBy(x => x).ToList();

            var index = (maxValue - cellValue) / maxValue;

            for (int i = 0; i < marks.Count; i++)
            {
                if (index > marks[i])
                {
                    return i;
                }
            }

            return 0;
        }
    }
}
