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
        private const string ApiBaseUrl = "http://c3-pra.test/MatchesApi/";

        public ApiReader()
        {
            httpClient = new HttpClient();
        }

        public MatchApi GetMatch(string Id)
        {
            if (string.IsNullOrEmpty(Id) || !int.TryParse(Id, out var match) || int.Parse(Id) <= 0)
            {
                return new MatchApi();
            }
            try
            {
                string url = $"{ApiBaseUrl}{Id}";
                var response = httpClient.GetStringAsync(url).Result;

                var responseContent = JsonSerializer.Deserialize<MatchApi>(response);

                return new MatchApi()
                {
                    team1_id = responseContent.team1_id,
                    team2_id = responseContent.team2_id,
                    team1_name = responseContent.team1_name,
                    team2_name = responseContent.team2_name,

                };
            }
            catch
            {
                return new MatchApi();
            }


            
        }

        public struct MatchApi
        {
            public int team1_id { get; set; }
            public int team2_id { get; set; }
            public string team1_name { get; set; }
            public string team2_name { get; set; }
        }
    }
}
