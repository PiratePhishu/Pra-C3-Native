using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Pra_C3_Native.Data
{
    internal class ApiReader
    {
        private readonly HttpClient httpClient;
        private const string ApiBaseUrl = "http://127.0.0.1:8000/MatchesApi";

        public ApiReader()
        {
            httpClient = new HttpClient();
        }

        public List<MatchApi> GetMatch()
        {
            try
            {
                var response = httpClient.GetAsync(ApiBaseUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var MatchesResponse = JsonSerializer.Deserialize<List<MatchApi>>(responseContent);
                    return MatchesResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return new List<MatchApi>();

        }

        public struct MatchApi
        {
            public int id {  get; set; }
            public int team1_id { get; set; }
            public int team2_id { get; set; }
            public int? team1_score { get; set; }
            public int? team2_score { get; set; }
            public TeamApi team1 { get; set; }
            public TeamApi team2 { get; set;}
        }

        public struct TeamApi
        {
            public int id { get; set; }
            public string name { get; set; }
        }
    }
}
