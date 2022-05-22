using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CountryClub.UnitTests
{
    public class UnitTest
    {
        private readonly Infrastructure.EFModel.CountryclubContext _context;
        //private readonly DbContext context;

        public UnitTest()
        {
            DbContextOptions<Infrastructure.EFModel.CountryclubContext> options = new DbContextOptionsBuilder<Infrastructure.EFModel.CountryclubContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;
            //dbOptions.UseSqlServer("Data Source=(localdb)\\Local;Initial Catalog=CountryClub;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            //options => options.UseSqlServer(builder.Configuration.GetConnectionString("Project")

            _context = new Infrastructure.EFModel.CountryclubContext(options);
        }

        [Fact]
        public async void Get_Mjesto()
        {
            //Arrange
            var sut = new MjestaRepository(_context);
            int _idMjesto = 9999;

            //Act
            var _result = await sut.GetMjestoById(_idMjesto);

            //Assert
            Assert.Null(_result);

        }

        [Fact]
        public async void Get_Home_Index()
        {
            //Arrange

            //Act
            var _result = new CountryClubMVC.Controllers.HomeController(null).Index();

            //Assert
            Assert.NotNull(_result);

        }
    }
}