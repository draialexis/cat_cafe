using AutoMapper;
using cat_cafe.Dto;
using cat_cafe.Entities;
using cat_cafe.Mappers;
using cat_cafe.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace cat_cafe.Controllers.Tests
{

    [TestClass()]
    public class BarsControllerTest
    {

        private readonly ILogger<BarsController> logger = new NullLogger<BarsController>();

        private readonly MapperConfiguration mapperConf = new(mapper => mapper.AddProfile(typeof(BarMapper)));

        private readonly DbContextOptions<CatCafeContext> options = new DbContextOptionsBuilder<CatCafeContext>()
                .UseInMemoryDatabase(databaseName: "CatCafeTests")
                .Options;

        private readonly Bar bleep = new()
        {
            Id = 1,
            Name = "bleep"
        };

        private readonly Bar bloop = new()
        {
            Id = 2,
            Name= "bloop"
        };


        private readonly BarDto bleepDto;

        private readonly BarDto bloopDto;

        private readonly IMapper mapper;

        private readonly CatCafeContext context;

        private readonly BarsController controller;

        public BarsControllerTest()
        {
            mapper = mapperConf.CreateMapper();
            context = new CatCafeContext(options);
            controller = new BarsController(context, mapper, logger);
            bleepDto = mapper.Map<BarDto>(bleep);
            bloopDto = mapper.Map<BarDto>(bloop);
        }


        [TestInitialize]
        public void BeforeEach()
        {
            context.Database.EnsureCreated();
            context.Bars.AddRange(bleep, bloop);
            context.SaveChanges();
        }

        [TestCleanup]
        public void AfterEach()
        {
            context.Database.EnsureDeleted();
        }

        [TestMethod()]
        public async Task GetBarsTest()
        {
            // control response type
            var actual = await controller.GetBars();
            actual.Result.Should().BeOfType<OkObjectResult>();

            // control response object
            var actualResult = actual.Result as OkObjectResult;
            actualResult.Should().NotBeNull();
            actualResult!.Value.Should()
                .BeEquivalentTo(new List<BarDto>() { bleepDto, bloopDto }.AsEnumerable());
        }

        [TestMethod()]
        public async Task GetBarTest()
        {
            // control response type
            var actual = await controller.GetBar(1);
            actual.Result.Should().BeOfType<OkObjectResult>();

            // control response object
            var actualResult = actual.Result as OkObjectResult;
            actualResult.Should().NotBeNull();
            actualResult!.Value.Should().BeEquivalentTo(bleepDto);
        }


        [TestMethod()]
        public async Task PutBarTest()
        {
            // Arrange
            BarDto blarp = new() { Id = 2, Name = "blarp" };

            // Act
            var responseType = await controller.PutBar(bloop.Id, blarp);

            // Assert
            responseType.Should().BeOfType<NoContentResult>();
            var actual = await controller.GetBar(bloop.Id);
            var actualResult = actual.Result as OkObjectResult;
            actualResult!.Value.Should().BeEquivalentTo(blarp);
        }


        [TestMethod()]
        public async Task PostBarTest()
        {
            // Arrange
            BarDto brrrrr = new() { Id = 3, Name = "brrrrr" };

            // Act
            var responseType = await controller.PostBar(brrrrr);

            // Assert
            responseType.Result.Should().BeOfType<CreatedAtActionResult>();
            var actual = await controller.GetBar(brrrrr.Id);
            var actualResult = actual.Result as OkObjectResult;
            actualResult!.Value.Should().BeEquivalentTo(brrrrr);
        }

        [TestMethod()]
        public async Task DeleteBarTest()
        {
            // Act
            var responseType = await controller.DeleteBar(bleep.Id);

            // Assert
            responseType.Should().BeOfType<NoContentResult>();

            var actual = await controller.GetBar(bleep.Id);
            actual.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}