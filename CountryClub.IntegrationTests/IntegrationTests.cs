using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace CountryClub.IntegrationTests
{
    public abstract class IntegrationTests
    {
        protected readonly HttpClient _client;

        public IntegrationTests()
        {
            var appFactory = new WebApplicationFactory<Program>();
            _client = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
/*            Dictionary<string, string> jsonValues = new Dictionary<string, string>();
            jsonValues.Add("Username", "mariohorvat");
            jsonValues.Add("Lozinka", "admin!");

            StringContent sc = new StringContent(JsonConvert.SerializeObject(jsonValues), UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("/Account/Prijava", sc);
            string content = await response.Content.ReadAsStringAsync();*/

            var requestBody = JsonConvert.SerializeObject(new DomainModel.AccountInfo
            {
                IdOsoba = 0,
                Username = "mariohorvat",
                Lozinka = "admin!"
            });
            var postRequest = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = _client.PostAsync("/Account/Prijava", postRequest).GetAwaiter().GetResult();
            var rawResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return null;
        }
    }
}
