using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Xema.Services.Infrastructure;

namespace Xema.Services
{
    public class CrossInhibitonSevice : ICrossInhibitionService
    {
        public void ProcessFile(IFormFile file)
        {
            throw new System.NotImplementedException();
        }

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
