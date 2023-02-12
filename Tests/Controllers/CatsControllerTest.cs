using AutoMapper;
using cat_cafe.Dto;
using cat_cafe.Entities;
using cat_cafe.Mappers;
using cat_cafe.Repositories;
using cat_cafe.WeSo;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace cat_cafe.Controllers.Tests
{

    [TestClass()]
    public class CatsControllerTest
    {

        private readonly ILogger<CatsController> logger = new NullLogger<CatsController>();

        private readonly MapperConfiguration mapperConf = new(mapper => mapper.AddProfile(typeof(CatMapper)));

        private readonly DbContextOptions<CatCafeContext> options = new DbContextOptionsBuilder<CatCafeContext>()
                .UseInMemoryDatabase(databaseName: "CatCafeTests")
                .Options;

        private readonly Cat alice = new()
        {
            Id = 1,
            Name = "Alice",
            Age = 5
        };

        private readonly Cat bob = new()
        {
            Id = 2,
            Name = "Bob",
            Age = 7
        };


        private readonly CatDto aliceDto;

        private readonly CatDto bobDto;

        private readonly IMapper mapper;

        private readonly CatCafeContext context;

        private readonly CatsController controller;

        public CatsControllerTest()
        {
            mapper = mapperConf.CreateMapper();
            context = new CatCafeContext(options);
            controller = new CatsController(context, mapper, logger, new WebSocketHandler(new List<System.Net.WebSockets.WebSocket>()));
            aliceDto = mapper.Map<CatDto>(alice);
            bobDto = mapper.Map<CatDto>(bob);
        }


        [TestInitialize]
        public void BeforeEach()
        {
            context.Database.EnsureCreated();
            context.Cats.AddRange(alice, bob);
            context.SaveChanges();
        }

        [TestCleanup]
        public void AfterEach()
        {
            context.Database.EnsureDeleted();
        }

        [TestMethod()]
        public async Task GetCatsV2Test()
        {
            // control response type
            var actual = await controller.GetCatsV2();
            actual.Result.Should().BeOfType<OkObjectResult>();

            // control response object
            var actualResult = actual.Result as OkObjectResult;
            actualResult.Should().NotBeNull();
            actualResult!.Value.Should()
                .BeEquivalentTo(new List<CatDto>() { aliceDto, bobDto }.AsEnumerable());
        }

        [TestMethod()]
        public async Task GetCatTest()
        {
            // control response type
            var actual = await controller.GetCat(1);
            actual.Result.Should().BeOfType<OkObjectResult>();

            // control response object
            var actualResult = actual.Result as OkObjectResult;
            actualResult.Should().NotBeNull();
            actualResult!.Value.Should().BeEquivalentTo(aliceDto);
        }


        [TestMethod()]
        public async Task PutCatTest()
        {
            // Arrange
            CatDto robert = new() { Id = 2, Name = "Robert" };

            // Act
            var responseType = await controller.PutCat(bob.Id, robert);

            // System.InvalidOperationException: 
            // The instance of entity type 'Cat' cannot be tracked because another instance
            // with the same key value for {'Id'} is already being tracked. 

            // ... this simple update should work out of the box, 
            // DbContext is already 'scoped' by default instead of singleton

            // Assert
            responseType.Should().BeOfType<NoContentResult>();
            // responseType.Result.Should().BeOfType<NoContentResult>();
            var actual = await controller.GetCat(bob.Id);
            var actualResult = actual.Result as OkObjectResult;
            actualResult!.Value.Should().BeEquivalentTo(robert);
        }


        [TestMethod()]
        public async Task PostCatTest()
        {
            // Arrange
            CatDto clyde = new() { Id = 3, Name = "Clyde" };

            // Act
            var responseType = await controller.PostCat(clyde);

            // Assert
            responseType.Result.Should().BeOfType<CreatedAtActionResult>();
            var actual = await controller.GetCat(clyde.Id);
            var actualResult = actual.Result as OkObjectResult;
            actualResult!.Value.Should().BeEquivalentTo(clyde);
        }

        [TestMethod()]
        public async Task DeleteCatTest()
        {
            // Act
            var responseType = await controller.DeleteCat(alice.Id);

            // Assert
            responseType.Should().BeOfType<NoContentResult>();

            var actual = await controller.GetCat(alice.Id);
            actual.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}