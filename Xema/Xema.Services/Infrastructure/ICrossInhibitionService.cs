using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Xema.Services.Infrastructure
{
    public interface ICrossInhibitionService
    {
        Task ProcessFile(IFormFile file);
    }
}
