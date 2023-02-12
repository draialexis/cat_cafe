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
    public class CustomersControllerTest
    {

        private readonly ILogger<CustomersController> logger = new NullLogger<CustomersController>();

        private readonly MapperConfiguration mapperConf = new(mapper => mapper.AddProfile(typeof(CustomerMapper)));

        private readonly DbContextOptions<CatCafeContext> options = new DbContextOptionsBuilder<CatCafeContext>()
                .UseInMemoryDatabase(databaseName: "CatCafeTests")
                .Options;

        private readonly Customer alice = new()
        {
            Id = 1,
            FullName = "Alice ",
            Age = 5
        };

        private readonly Customer bob = new()
        {
            Id = 2,
            FullName = "Bob",
            Age = 7
        };


        private readonly CustomerDto aliceDto;

        private readonly CustomerDto bobDto;

        private readonly IMapper mapper;

        private readonly CatCafeContext context;

        private readonly CustomersController controller;

        public CustomersControllerTest()
        {
            mapper = mapperConf.CreateMapper();
            context = new CatCafeContext(options);
            controller = new CustomersController(context, mapper, logger);
            aliceDto = mapper.Map<CustomerDto>(alice);
            bobDto = mapper.Map<CustomerDto>(bob);
        }


        [TestInitialize]
        public void BeforeEach()
        {
            context.Database.EnsureCreated();
            context.Customers.AddRange(alice, bob);
            context.SaveChanges();
        }

        [TestCleanup]
        public void AfterEach()
        {
            context.Database.EnsureDeleted();
        }

        [TestMethod()]
        public async Task GetCustomersTest()
        {
            // control response type
            var actual = await controller.GetCustomers();
            actual.Result.Should().BeOfType<OkObjectResult>();

            // control response object
            var actualResult = actual.Result as OkObjectResult;
            actualResult.Should().NotBeNull();
            actualResult!.Value.Should()
                .BeEquivalentTo(new List<CustomerDto>() { aliceDto, bobDto }.AsEnumerable());
        }

        [TestMethod()]
        public async Task GetCustomerTest()
        {
            // control response type
            var actual = await controller.GetCustomer(1);
            actual.Result.Should().BeOfType<OkObjectResult>();

            // control response object
            var actualResult = actual.Result as OkObjectResult;
            actualResult.Should().NotBeNull();
            actualResult!.Value.Should().BeEquivalentTo(aliceDto);
        }


        [TestMethod()]
        public async Task PutCustomerTest()
        {
            // Arrange
            CustomerDto jhone = new() { Id = 2, FullName = "bob" };

            // Act
            var responseType = await controller.PutCustomer(bob.Id, jhone);

            // Assert
            responseType.Should().BeOfType<NoContentResult>();
            var actual = await controller.GetCustomer(bob.Id);
            var actualResult = actual.Result as OkObjectResult;
            actualResult!.Value.Should().BeEquivalentTo(jhone);
        }


        [TestMethod()]
        public async Task PostCustomerTest()
        {
            // Arrange
            CustomerDto clyde = new() { Id = 3, FullName = "Clyde" };

            // Act
            var responseType = await controller.PostCustomer(clyde);

            // Assert
            responseType.Result.Should().BeOfType<CreatedAtActionResult>();
            var actual = await controller.GetCustomer(clyde.Id);
            var actualResult = actual.Result as OkObjectResult;
            actualResult!.Value.Should().BeEquivalentTo(clyde);
        }

        [TestMethod()]
        public async Task DeleteCustomerTest()
        {
            // Act
            var responseType = await controller.DeleteCustomer(alice.Id);

            // Assert
            responseType.Should().BeOfType<NoContentResult>();

            var actual = await controller.GetCustomer(alice.Id);
            actual.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}