using Microsoft.AspNetCore.Http;

namespace Xema.Services.Infrastructure
{
    public interface ICrossInhibitionService
    {
        void ProcessFile(IFormFile file);
    }
}
