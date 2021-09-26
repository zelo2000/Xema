using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xema.Services.Infrastructure;

namespace Xema.Services
{
    public class CrossInhibitonSevice : ICrossInhibitionService
    {
        public async Task ProcessFile(IFormFile file)
        {
            var result = new List<List<string>>();
            using var reader = new StreamReader(file.OpenReadStream());
            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();
                result.Add(line.Split(';').ToList());
            }

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
