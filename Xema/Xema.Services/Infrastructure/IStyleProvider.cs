using SpreadsheetLight;

namespace Xema.Services.Infrastructure
{
    public interface IStyleProvider
    {
        public SLStyle GetGroupHeaderStyle();

        public SLStyle GetGroupCellStyle();

        public SLStyle GetGroupLastCellStyle();

        public SLStyle GetFilledCellStyle(System.Drawing.Color color);
    }
}
