using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace BenchmarkingWeb
{
    internal class MongoRestClient
    {
        private static HttpClient client = new HttpClient(new SocketsHttpHandler{
            PooledConnectionIdleTimeout = TimeSpan.FromSeconds(60),
            PooledConnectionLifetime = TimeSpan.FromSeconds(60)
        });

        private static readonly RandomGenerator randomGenerator = new RandomGenerator();
        
        private const string port = "7050";

        public MongoRestClient()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TournamentDto> PostSampleTournamentPayloadAsync()
        {
            var payload = string.Concat("{\"name\": \"", randomGenerator.GetRandomString(randomGenerator.GetRandomInt(25, 50)) ,"\", \"description\": \"", randomGenerator.GetRandomString(randomGenerator.GetRandomInt(100, 200)) ,"\"}");
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var url = $"https://localhost:{port}/api/TournamentMongo" ;
            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadFromJsonAsync<TournamentDto>();
            return result;
        }

        public async Task<TournamentDto> GetSampleTournamentPayloadAsync(string id)
        {
            try
            {
                var url = $"https://localhost:{port}/api/TournamentMongo/{id}";
                var results = await client.GetFromJsonAsync<TournamentDto>(url);

                if(results.Id != id)
                {
                    throw new Exception($"{id} not found");
                }
                
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{id} not found - {ex.Message}");
                return null;
            }
        } 

        public async Task<List<TournamentDto>> GetSampleTournamentPayloadAsync()
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"https://localhost:{port}/api/TournamentMongo";
                var results = await client.GetFromJsonAsync<List<TournamentDto>>(url);
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }
    }
}
