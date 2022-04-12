using Prototyping.Domain.Models;
using System.Net.Http.Json;
using System.Text;

namespace BenchmarkingWeb
{
    internal class TournamentDto: ITournament{
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    internal class RestClient
    {
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler{UseProxy=false});

        private static readonly RandomGenerator randomGenerator = new RandomGenerator();
        
        private const string port = "7050";

        public RestClient()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TournamentDto> PostSampleTournamentPayloadAsync()
        {
            var payload = string.Concat("{\"name\": \"", randomGenerator.GetRandomString(randomGenerator.GetRandomInt(25, 50)) ,"\", \"description\": \"", randomGenerator.GetRandomString(randomGenerator.GetRandomInt(100, 200)) ,"\"}");
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var url = $"https://localhost:{port}/api/Tournament" ;
            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadFromJsonAsync<TournamentDto>();
            return result;
        }

        public async Task<TournamentDto> GetSampleTournamentPayloadAsync(string id)
        {
            try
            {
                var url = $"https://localhost:{port}/api/Tournament/{id}";
                var results = await client.GetFromJsonAsync<TournamentDto>(url);
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{id} not found - {ex.Message}");
                return null;
            }
        } 

        public async Task<TournamentDto> GetSampleTournamentPayloadNoTrackingAsync(string id)
        {
            try
            {
                var url = $"https://localhost:{port}/api/Tournament/noTracking/{id}";
                var results = await client.GetFromJsonAsync<TournamentDto>(url);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TournamentDto>> GetSampleTournamentPayloadAsync()
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"https://localhost:{port}/api/Tournament";
                var results = await client.GetFromJsonAsync<List<TournamentDto>>(url);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

     internal class MongoRestClient
    {
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler{UseProxy=false});

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

    internal class RandomGenerator{
        private static Random ran = new Random();

        private const string b = "abcdefghijklmnopqrstuvwxyz0123456789          !@#$%^&*~";

        private static int len = b.Length;

        public string GetRandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(b[ran.Next(len)]);
            }
            return sb.ToString();
        }

        public int GetRandomInt(int min, int max) => ran.Next(min, max);
    }
}
