using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xema.ClusterAPI.Models;
using Xema.Core.Enums;
using Xema.Core.Models;
using Xema.Core.Models.Configuration;

namespace Xema.ClusterAPI
{
    public class ClusterApiClient : IClusterApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ClusterApiSettings _clusterApiSettings;

        public ClusterApiClient(IHttpClientFactory httpClientFactory, ClusterApiSettings clusterApiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _clusterApiSettings = clusterApiSettings;
        }

        public async Task<CrossInhibitorRawDataModel> Cluster(IFormFile file)
        {
            // Set up http client
            var httpClient = _httpClientFactory.CreateClient(_clusterApiSettings.Name);
            var formContent = PrepareContent(file);

            // Make a request
            var responseMessage = await httpClient.PostAsync(_clusterApiSettings.ClusterEndpoint, formContent);
            responseMessage.EnsureSuccessStatusCode();

            // Response processing
            var response = await MapResponse(responseMessage);
            return response;
        }

        private MultipartFormDataContent PrepareContent(IFormFile file)
        {
            var fileStreamContent = new StreamContent(file.OpenReadStream());
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            var formContent = new MultipartFormDataContent
            {
                { fileStreamContent, "file", file.FileName }
            };

            return formContent;
        }

        private async Task<CrossInhibitorRawDataModel> MapResponse(HttpResponseMessage responseMessage)
        {
            var responseAsString = await responseMessage.Content.ReadAsStringAsync();
            var clusterResponce = JsonConvert.DeserializeObject<ClusterResponce>(responseAsString);

            // Map labels
            var antigenLabels = clusterResponce.Colors.Keys.ToList();
            var markedAntigenLabels = clusterResponce.Colors.FirstOrDefault().Value.Keys.ToList();

            // Map clusters
            var clusters = new Dictionary<string, List<string>>();
            foreach (var cluster in clusterResponce.Clusters)
            {
                var key = cluster.Value.ToString();
                if (clusters.ContainsKey(key))
                {
                    clusters[key].Add(cluster.Key);

                    clusters[key] = clusters[key]
                        .OrderBy(x => x)
                        .ToList();
                }
                else
                {
                    clusters.Add(key, new List<string> { cluster.Key });
                }
            }

            // Add wrong data cluster
            if (clusterResponce.Wrong.Count > 0)
            {
                clusters["Unknown group"] = clusterResponce.Wrong.Keys.ToList();
            }

            // Create list
            var crossInhibitionIndexes = new List<List<List<IndexCell>>>();
            foreach (var key in clusters.Keys)
            {
                crossInhibitionIndexes.Add(new List<List<IndexCell>>());
            }

            // Map values
            foreach (var antigenLabel in clusterResponce.Colors.Keys)
            {
                var row = new List<IndexCell>();
                foreach (var markedAntigenLabel in clusterResponce.Colors[antigenLabel].Keys)
                {
                    var initialValue = clusterResponce.InitialValue[antigenLabel][markedAntigenLabel];
                    var index = clusterResponce.Indexes[antigenLabel][markedAntigenLabel];
                    var color = clusterResponce.Colors[antigenLabel][markedAntigenLabel];

                    var cell = new IndexCell
                    {
                        Value = initialValue,
                        MarkerIndex = index,
                        MarkerColor = color
                    };

                    row.Add(cell);
                }

                var clusterIndex = clusterResponce.Clusters[antigenLabel];
                crossInhibitionIndexes[clusterIndex].Add(row);
            }

            // Map wrong data
            foreach (var antigenLabel in clusterResponce.Wrong.Keys)
            {
                var row = new List<IndexCell>();
                foreach (var markedAntigenLabel in clusterResponce.Wrong[antigenLabel].Keys)
                {
                    var initialValue = clusterResponce.InitialValue[antigenLabel][markedAntigenLabel];
                    var index = clusterResponce.Wrong[antigenLabel][markedAntigenLabel];

                    var cell = new IndexCell
                    {
                        Value = initialValue,
                        MarkerIndex = index,
                        MarkerColor = InhibitionColors.None
                    };

                    row.Add(cell);
                }

                // Last cluster should be "Unknown group"
                crossInhibitionIndexes[crossInhibitionIndexes.Count - 1].Add(row);
            }

            // Split lables by cluster
            var antigenLabelsResult = new List<List<string>>();
            var skip = 0;
            foreach (var clusterIndexes in crossInhibitionIndexes)
            {
                var labelByCluster = antigenLabels
                    .Skip(skip)
                    .Take(clusterIndexes.Count)
                    .ToList();

                antigenLabelsResult.Add(labelByCluster);
                skip += clusterIndexes.Count;
            }

            // Add wrong data antigenLabels
            if (clusterResponce.Wrong.Count > 0)
            {
                antigenLabelsResult[antigenLabelsResult.Count - 1] = clusterResponce.Wrong.Keys.ToList();
            }

            var result = new CrossInhibitorRawDataModel
            {
                AntigenLabels = antigenLabelsResult,
                MarkedAntigenLabels = markedAntigenLabels,
                Clusters = clusters,
                CrossInhibitionIndexes = crossInhibitionIndexes
            };

            return result;
        }
    }
}
