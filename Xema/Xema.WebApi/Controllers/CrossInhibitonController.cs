using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xema.Services.Infrastructure;

namespace Xema.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrossInhibitonController : ControllerBase
    {
        private readonly ICrossInhibitionService _crossInhibitionService;

        public CrossInhibitonController(ICrossInhibitionService crossInhibitionService)
        {
            _crossInhibitionService = crossInhibitionService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessFile(IFormFile file)
        {
            await _crossInhibitionService.ProcessFile(file);
            return Ok();
        }
    }
}
