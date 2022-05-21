using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using FluentAssertions;

namespace CountryClub.IntegrationTests
{
    public class IntegrationTests
    {
        private readonly HttpClient _client;

        public IntegrationTests()
        {
            var appFactory = new WebApplicationFactory<Program>();
            _client = appFactory.CreateClient();
        }

        [Fact]
        public async Task Get_Default_Index()
        {
            var response = await _client.GetAsync("");
            /*var stringResult = await response.Content.ReadAsStringAsync();

            Assert.Equal("kek!", stringResult);*/
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}