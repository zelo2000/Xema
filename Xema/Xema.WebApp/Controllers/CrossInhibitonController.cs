using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xema.Core.Models;
using Xema.Services.Infrastructure;

namespace Xema.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrossInhibitonController : ControllerBase
    {
        private readonly ICrossInhibitionService _crossInhibitionService;
        private readonly IExcelService _excelService;

        public CrossInhibitonController(ICrossInhibitionService crossInhibitionService, IExcelService excelService)
        {
            _crossInhibitionService = crossInhibitionService;
            _excelService = excelService;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<CrossInhibitorRawDataModel>> UploadFile([FromForm] IFormFile file)
        {
            var result = await _crossInhibitionService.Cluster(file);
            return Ok(result);
        }

        [HttpPost("export-xlsx")]
        public ActionResult DownloadExcel(CrossInhibitorRawDataModel dataModel)
        {
            var result = _excelService.GetFileStream(dataModel);
            return File(result.GetBuffer(), "application/octet-stream", "test.xlsx");
        }
    }
}
