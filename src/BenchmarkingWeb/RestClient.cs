using Prototyping.Domain.Models;
using System.Net.Http.Json;
using System.Text;

namespace BenchmarkingWeb
{
    internal class RestClient
    {
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler{UseProxy=false});

        private static readonly RandomGenerator randomGenerator = new RandomGenerator();
        private const string port = "7050";
        public async Task<Tournament> PostSampleTournamentPayloadAsync()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
            var payload = string.Concat("{\"name\": \"", randomGenerator.GetRandomString(randomGenerator.GetRandomInt(25, 50)) ,"\", \"description\": \"", randomGenerator.GetRandomString(randomGenerator.GetRandomInt(100, 200)) ,"\"}");
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var url = $"https://localhost:{port}/api/Tournament" ;
            var response = await client.PostAsync(url, content);
            return await response.Content.ReadFromJsonAsync<Tournament>();
        }

        public async Task<Tournament> GetSampleTournamentPayloadAsync(string id)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"https://localhost:{port}/api/Tournament/{id}";
                var results = await client.GetFromJsonAsync<Tournament>(url);
                return results;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Tournament> GetSampleTournamentPayloadNoTrackingAsync(string id)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"https://localhost:{port}/api/Tournament/noTracking/{id}";
                var results = await client.GetFromJsonAsync<Tournament>(url);
                return results;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Tournament>> GetSampleTournamentPayloadAsync()
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"https://localhost:{port}/api/Tournament";
                var results = await client.GetFromJsonAsync<List<Tournament>>(url);
                return results;
            }
            catch (Exception)
            {
                return null;
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
