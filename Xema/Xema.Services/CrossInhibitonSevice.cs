using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xema.ClusterAPI;
using Xema.Core.Models;
using Xema.Services.Infrastructure;

namespace Xema.Services
{
    public class CrossInhibitonSevice : ICrossInhibitionService
    {
        private readonly IClusterApiClient _clusterApiClient;

        public CrossInhibitonSevice(IClusterApiClient clusterApiClient)
        {
            _clusterApiClient = clusterApiClient;
        }

        public async Task<CrossInhibitorRawDataModel> Cluster(IFormFile file)
        {
            var response = await _clusterApiClient.Cluster(file);
            return response;
        }


        //public async Task<CrossInhibitorRawDataModel> UploadFile(IFormFile file)
        //{
        //    var crossInhibitorRawDataModel = new CrossInhibitorRawDataModel();

        //    using var reader = new StreamReader(file.OpenReadStream());
        //    while (reader.Peek() >= 0)
        //    {
        //        // read line from file
        //        var line = await reader.ReadLineAsync();

        //        //Split line by ; separator
        //        var splitedLine = line.Split(';').ToList();

        //        // If first row with header was readed
        //        if (crossInhibitorRawDataModel.MarkedAntigenLabels.Count > 0)
        //        {
        //            if (splitedLine.Count > 2)
        //            {
        //                // Antigen label
        //                var label = splitedLine[1];

        //                if (label.Contains("blank"))
        //                {
        //                    // Get blank indexes
        //                    crossInhibitorRawDataModel.BlankIndexes = splitedLine.Skip(2)
        //                        .Select(l => double.Parse(l))
        //                        .ToList();
        //                }
        //                else
        //                {
        //                    crossInhibitorRawDataModel.AntigenLabels.Add(label);

        //                    // Skip neat and label column
        //                    splitedLine = splitedLine.Skip(2).ToList();

        //                    // Parse indexes to IndexCell
        //                    var lineInhibitonIndexes = new List<IndexCell>();
        //                    for (var i = 0; i < splitedLine.Count; i++)
        //                    {
        //                        var isParsed = double.TryParse(splitedLine[i], out var n);
        //                        if (isParsed)
        //                        {
        //                            var cell = DrawCell(n, crossInhibitorRawDataModel.BlankIndexes[i]);
        //                            lineInhibitonIndexes.Add(cell);
        //                        }
        //                    }

        //                    crossInhibitorRawDataModel.CrossInhibitionIndexes.Add(lineInhibitonIndexes);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // Add first row with labels
        //            crossInhibitorRawDataModel.MarkedAntigenLabels = splitedLine.Skip(2).ToList();
        //        }
        //    }

        //    WriteCellPropertyToFile(crossInhibitorRawDataModel, "drawn_color.csv");
        //    WriteCellPropertyToFile(crossInhibitorRawDataModel, "drawn_index.csv", true);

        //    return crossInhibitorRawDataModel;
        //}

        //private IndexCell DrawCell(double cellValue, double maxValue)
        //{
        //    var cell = new IndexCell
        //    {
        //        Value = cellValue
        //    };

        //    var index = (maxValue - cell.Value) / maxValue;

        //    cell.MarkerIndex = Math.Round(index, 2);
        //    if (index > 0.75)
        //    {
        //        cell.MarkerColor = InhibitionColors.DarkGreen;
        //    }
        //    else if (index > 0.5)
        //    {
        //        cell.MarkerColor = InhibitionColors.LightGreen;
        //    }
        //    else
        //    {
        //        cell.MarkerColor = InhibitionColors.White;
        //    }

        //    return cell;
        //}

        //private void WriteCellPropertyToFile(CrossInhibitorRawDataModel model, string path, bool isIndex = false)
        //{
        //    if (!File.Exists(path))
        //    {
        //        File.Create(path).Close();
        //    }

        //    var stringBuilder = new StringBuilder();
        //    stringBuilder.AppendLine("label," + string.Join(',', model.MarkedAntigenLabels));

        //    for (var i = 0; i < model.AntigenLabels.Count; i++)
        //    {
        //        stringBuilder.Append(model.AntigenLabels[i]);
        //        stringBuilder.Append(',');
        //        var indexes = model.CrossInhibitionIndexes[i]
        //            .Select(x => isIndex ? x.MarkerIndex : (int)x.MarkerColor);
        //        stringBuilder.Append(string.Join(',', indexes));
        //        stringBuilder.AppendLine();
        //    }

        //    File.WriteAllText(path, stringBuilder.ToString());
        //}
    }
}
