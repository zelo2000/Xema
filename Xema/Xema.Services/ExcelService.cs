using SpreadsheetLight;
using System.IO;
using System.Linq;
using Xema.Core.Enums;
using Xema.Core.Models;
using Xema.Services.Infrastructure;

namespace Xema.Services
{
    public class ExcelService : IExcelService
    {
        private readonly IStyleProvider _styleProvider;

        public ExcelService(IStyleProvider styleProvider)
        {
            _styleProvider = styleProvider;
        }

        public MemoryStream GetFileStream(CrossInhibitorRawDataModel dataModel)
        {
            var sl = new SLDocument();

            var rowIndex = 2;
            var columnIndex = 2;

            var groupHeaderStyle = _styleProvider.GetGroupHeaderStyle();
            var groupCellStyle = _styleProvider.GetGroupCellStyle();
            var lastGroupCellStyle = _styleProvider.GetGroupLastCellStyle();
            var darkGreenStyle = _styleProvider.GetFilledCellStyle(System.Drawing.Color.FromArgb(0, 176, 79));
            var lightGreenStyle = _styleProvider.GetFilledCellStyle(System.Drawing.Color.FromArgb(146, 208, 80));

            // Add clusters to file
            foreach (var keyValue in dataModel.Clusters)
            {
                var isInt = int.TryParse(keyValue.Key, out var number);
                var label = keyValue.Key;
                if (isInt)
                {
                    label = $"Group {number + 1}";
                }

                sl.SetCellValue(rowIndex, columnIndex, label);
                sl.SetCellStyle(rowIndex, columnIndex, groupHeaderStyle);

                rowIndex++;
                for (var i = 0; i < keyValue.Value.Count; i++)
                {
                    var item = keyValue.Value[i];
                    sl.SetCellValue(rowIndex, columnIndex, item);

                    if (i == keyValue.Value.Count - 1)
                    {
                        // Add border in the bottom to close table
                        sl.SetCellStyle(rowIndex, columnIndex, lastGroupCellStyle);
                    }
                    else
                    {
                        sl.SetCellStyle(rowIndex, columnIndex, groupCellStyle);
                    }

                    rowIndex++;
                }

                columnIndex++;
                rowIndex = 2;
            }

            var intialRawDataRow = rowIndex + dataModel.Clusters.Values.ToList().Max(x => x.Count) + 2;

            // Add marked antigen labels to file
            rowIndex = intialRawDataRow;
            columnIndex = 3;
            foreach (var label in dataModel.MarkedAntigenLabels)
            {
                sl.SetCellValue(rowIndex, columnIndex, label);
                columnIndex++;
            }

            // Add antigen labels to file
            rowIndex++;
            columnIndex = 1;
            for (var i = 0; i < dataModel.AntigenLabels.Count; i++)
            {
                var keyValue = dataModel.Clusters.ElementAt(i);
                var isInt = int.TryParse(keyValue.Key, out var number);
                var groupLabel = keyValue.Key;
                if (isInt)
                {
                    groupLabel = $"Group {number + 1}";
                }

                sl.SetCellValue(rowIndex, columnIndex, groupLabel);
                columnIndex++;

                var labelGroup = dataModel.AntigenLabels[i];
                foreach (var label in labelGroup)
                {
                    sl.SetCellValue(rowIndex, columnIndex, label);
                    rowIndex++;
                }

                rowIndex++;
                columnIndex = 1;
            }

            // Add cross inhibition indexes to file
            rowIndex = intialRawDataRow + 1;
            columnIndex = 3;
            foreach (var clusterGroup in dataModel.CrossInhibitionIndexes)
            {
                foreach (var row in clusterGroup)
                {
                    foreach (var cell in row)
                    {
                        sl.SetCellValue(rowIndex, columnIndex, cell.Value);
                        switch (cell.MarkerColor)
                        {
                            case InhibitionColors.DarkGreen:
                                sl.SetCellStyle(rowIndex, columnIndex, darkGreenStyle);
                                break;
                            case InhibitionColors.LightGreen:
                                sl.SetCellStyle(rowIndex, columnIndex, lightGreenStyle);
                                break;
                            default:
                                break;
                        }

                        columnIndex++;
                    }

                    rowIndex++;
                    columnIndex = 3;
                }

                rowIndex++;
            }

            using var stream = new MemoryStream();
            sl.SaveAs(stream);

            return stream;
        }
    }
}
