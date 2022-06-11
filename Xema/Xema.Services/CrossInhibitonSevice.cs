using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xema.ClusterAPI;
using Xema.Core.Models;
using Xema.Services.Infrastructure;

namespace Xema.Services
{
    public class CrossInhibitonSevice : ICrossInhibitionService
    {
        private readonly IClusterApiClient _clusterApiClient;

        public CrossInhibitonSevice(IClusterApiClient clusterApiClient)
        {
            _clusterApiClient = clusterApiClient;
        }

        public async Task<CrossInhibitorRawDataModel> Cluster(IFormFile file)
        {
            var response = await _clusterApiClient.Cluster(file);
            return response;
        }
    }
}
