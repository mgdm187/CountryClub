using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryClub.IntegrationTests
{
    public class GetControllerTests : IntegrationTests
    {
        [Fact]
        public async Task Get_Default_Index()
        {
            var response = await _client.GetAsync("");
            /*var stringResult = await response.Content.ReadAsStringAsync();

            Assert.Equal("kek!", stringResult);*/
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Default_Clanarine()
        {
            var response = await _client.GetAsync("/Clanarine");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Default_Usluge()
        {
            var response = await _client.GetAsync("/Usluge");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Default_Mjesta()
        {
            var response = await _client.GetAsync("/Mjesta");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Default_Osobe()
        {
            await AuthenticateAsync();

            var response = await _client.GetAsync("/Osobe");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Default_Rezervacije()
        {
            var response = await _client.GetAsync("/Rezervacije");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Default_Racuni()
        {
            var response = await _client.GetAsync("/Racuni");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Default_MojRacun()
        {
            await AuthenticateAsync();

            var response = await _client.GetAsync("/Account/MojRacun");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}
