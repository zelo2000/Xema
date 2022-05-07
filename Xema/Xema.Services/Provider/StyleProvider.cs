using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using Xema.Services.Infrastructure;

namespace Xema.Services.Provider
{
    public class StyleProvider : IStyleProvider
    {
        public SLStyle GetGroupHeaderStyle()
        {
            var style = new SLStyle();
            style.SetTopBorder(BorderStyleValues.Thin, SLThemeColorIndexValues.Dark1Color);
            style.SetLeftBorder(BorderStyleValues.Thin, SLThemeColorIndexValues.Dark1Color);
            style.SetRightBorder(BorderStyleValues.Thin, SLThemeColorIndexValues.Dark1Color);
            style.SetBottomBorder(BorderStyleValues.Thin, SLThemeColorIndexValues.Dark1Color);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);

            return style;
        }

        public SLStyle GetGroupCellStyle()
        {
            var style = new SLStyle();
            style.SetLeftBorder(BorderStyleValues.Thin, SLThemeColorIndexValues.Dark1Color);
            style.SetRightBorder(BorderStyleValues.Thin, SLThemeColorIndexValues.Dark1Color);

            return style;
        }

        public SLStyle GetGroupLastCellStyle()
        {
            var style = new SLStyle();
            style.SetLeftBorder(BorderStyleValues.Thin, SLThemeColorIndexValues.Dark1Color);
            style.SetRightBorder(BorderStyleValues.Thin, SLThemeColorIndexValues.Dark1Color);
            style.SetBottomBorder(BorderStyleValues.Thin, SLThemeColorIndexValues.Dark1Color);

            return style;
        }

        public SLStyle GetFilledCellStyle(System.Drawing.Color color)
        {
            var style = new SLStyle();
            style.Fill.SetPattern(PatternValues.Solid, color, color);

            return style;
        }
    }
}
