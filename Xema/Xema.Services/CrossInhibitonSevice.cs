using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xema.Core.Enums;
using Xema.Core.Models;
using Xema.Services.Infrastructure;

namespace Xema.Services
{
    public class CrossInhibitonSevice : ICrossInhibitionService
    {
        public async Task<CrossInhibitorRawDataModel> UploadFile(IFormFile file)
        {
            var crossInhibitorRawDataModel = new CrossInhibitorRawDataModel();

            using var reader = new StreamReader(file.OpenReadStream());
            while (reader.Peek() >= 0)
            {
                // read line from file
                var line = await reader.ReadLineAsync();

                //Split line by ; separator
                var splitedLine = line.Split(';').ToList();

                // If first row with header was readed
                if (crossInhibitorRawDataModel.MarkedAntigenLabels.Count > 0)
                {
                    if (splitedLine.Count > 2)
                    {
                        // Antigen label
                        var label = splitedLine[1];

                        if (label.Contains("blank"))
                        {
                            // Get blank indexes
                            crossInhibitorRawDataModel.BlankIndexes = splitedLine.Skip(2)
                                .Select(l => double.Parse(l))
                                .ToList();
                        }
                        else
                        {
                            crossInhibitorRawDataModel.AntigenLabels.Add(label);

                            // Skip neat and label column
                            splitedLine = splitedLine.Skip(2).ToList();

                            // Parse indexes to IndexCell
                            var lineInhibitonIndexes = new List<IndexCell>();
                            for (var i = 0; i < splitedLine.Count; i++)
                            {
                                var isParsed = double.TryParse(splitedLine[i], out var n);
                                if (isParsed)
                                {
                                    var incexCell = new IndexCell
                                    {
                                        Value = n,
                                        MarkerIndex = DrawCell(n, crossInhibitorRawDataModel.BlankIndexes[i]),
                                    };

                                    lineInhibitonIndexes.Add(incexCell);
                                }
                            }

                            crossInhibitorRawDataModel.CrossInhibitionIndexes.Add(lineInhibitonIndexes);
                        }
                    }
                }
                else
                {
                    // Add first row with labels
                    crossInhibitorRawDataModel.MarkedAntigenLabels = splitedLine.Skip(2).ToList();
                }
            }

            return crossInhibitorRawDataModel;
        }

        private static InhibitionColors DrawCell(double cellValue, double maxValue)
        {
            var index = (maxValue - cellValue) / maxValue;

            if (index > 0.75)
            {
                return InhibitionColors.DarkGreen;
            }
            else if (index > 0.5)
            {
                return InhibitionColors.LightGreen;
            }

            return InhibitionColors.White;
        }
    }
}
