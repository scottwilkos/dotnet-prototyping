using Prototyping.Common.Dtos;
using System.Net.Http.Json;
using System.Text;

namespace Prototyping.Common
{
    public class MongoRestClient
    {
        private readonly string _baseUrl;

        private static readonly HttpClient Client = new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = TimeSpan.FromSeconds(60),
            PooledConnectionLifetime = TimeSpan.FromSeconds(60)
        });

        private static readonly RandomGenerator RandomGenerator = new RandomGenerator();

        public MongoRestClient(string baseUrl)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            this._baseUrl = baseUrl;
        }

        public async Task<TournamentDto?> PostSampleTournamentPayloadAsync()
        {
            var payload = string.Concat("{\"name\": \"", RandomGenerator.GetRandomString(RandomGenerator.GetRandomInt(25, 50)), "\", \"description\": \"", RandomGenerator.GetRandomString(RandomGenerator.GetRandomInt(100, 200)), "\"}");
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var url = $"{_baseUrl}/api/TournamentMongo";
            var response = await Client.PostAsync(url, content);
            var result = await response.Content.ReadFromJsonAsync<TournamentDto>();
            return result;
        }

        public async Task<TournamentDto?> GetSampleTournamentPayloadAsync(string id)
        {
            try
            {
                var url = $"{_baseUrl}/api/TournamentMongo/{id}";
                var results = await Client.GetFromJsonAsync<TournamentDto>(url);

                if (results.Id != id)
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

        public async Task<TournamentDto?> GetSampleTournamentNoCqrs(string id)
        {
            try
            {
                var url = $"{_baseUrl}/api/TournamentMongo/noHandler/{id}";
                var results = await Client.GetFromJsonAsync<TournamentDto>(url);

                if (results.Id != id)
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

        public async Task<List<TournamentDto>?> GetSampleTournamentPayloadAsync()
        {
            try
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"{_baseUrl}/api/TournamentMongo";
                var results = await Client.GetFromJsonAsync<List<TournamentDto>>(url);
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
