using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xema.Core.Models;

namespace Xema.Services.Infrastructure
{
    public interface ICrossInhibitionService
    {
        Task<CrossInhibitorRawDataModel> Cluster(IFormFile file);
    }
}
