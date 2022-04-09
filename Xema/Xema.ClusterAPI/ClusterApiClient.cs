using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
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
            var clusters = new Dictionary<int, List<string>>();
            foreach (var cluster in clusterResponce.Clusters)
            {
                if (clusters.ContainsKey(cluster.Value))
                {
                    clusters[cluster.Value].Add(cluster.Key);

                    clusters[cluster.Value] = clusters[cluster.Value]
                        .OrderBy(x => x)
                        .ToList();
                }
                else
                {
                    clusters.Add(cluster.Value, new List<string> { cluster.Key });
                }
            }

            // Map values
            var crossInhibitionIndexes = new List<List<IndexCell>>();
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

                crossInhibitionIndexes.Add(row);
            }

            var result = new CrossInhibitorRawDataModel
            {
                AntigenLabels = antigenLabels,
                MarkedAntigenLabels = markedAntigenLabels,
                Clusters = clusters,
                CrossInhibitionIndexes = crossInhibitionIndexes
            };

            return result;
        }
    }
}
