using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult ProcessFile(IFormFile file)
        {
            return Ok();
        }
    }
}
