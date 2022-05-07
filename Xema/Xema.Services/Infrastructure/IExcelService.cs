using System.IO;
using Xema.Core.Models;

namespace Xema.Services.Infrastructure
{
    public interface IExcelService
    {
        MemoryStream GetFileStream(CrossInhibitorRawDataModel dataModel);
    }
}
