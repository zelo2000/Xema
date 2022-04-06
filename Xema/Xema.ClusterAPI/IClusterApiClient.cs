using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xema.Core.Models;

namespace Xema.ClusterAPI
{
    public interface IClusterApiClient
    {
        public Task<CrossInhibitorRawDataModel> Cluster(IFormFile file);
    }
}
