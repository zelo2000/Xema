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

        public CrossInhibitonController(ICrossInhibitionService crossInhibitionService)
        {
            _crossInhibitionService = crossInhibitionService;
        }

        [HttpPost]
        public async Task<ActionResult<CrossInhibitorRawDataModel>> UploadFile([FromForm] IFormFile file)
        {
            var result = await _crossInhibitionService.Cluster(file);
            return Ok(result);
        }
    }
}
