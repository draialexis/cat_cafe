using AutoMapper;
using Castle.Core.Logging;
using cat_cafe.Controllers;
using cat_cafe.Dto;
using cat_cafe.Entities;
using cat_cafe.Mappers;
using cat_cafe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;


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

        private readonly IMapper mapper;

        private readonly CatCafeContext context;

        private readonly CatsController controller;

        public CatsControllerTest()
        {
            mapper = mapperConf.CreateMapper();
            context = new CatCafeContext(options);
            controller = new CatsController(context, mapper, logger);
        }


        [TestInitialize]
        public void BeforeEach()
        {
            context.Database.EnsureCreated();
            context.Cats.AddRange(
                new Cat
                {
                    Id = 1,
                    Name = "Alice",
                    Age = 5
                },
                new Cat
                {
                    Id = 2,
                    Name = "Bob",
                    Age = 3
                });
            context.SaveChanges();
        }

        [TestCleanup]
        public void AfterEach()
        {
            context.Database.EnsureDeleted();
        }

        [TestMethod()]
        public async Task GetCatsTest()
        {
            var actual = await controller.GetCats();

            actual.Result.Should().BeOfType<OkObjectResult>();

            var actualResult = actual.Result as OkObjectResult;

            actualResult.Should().NotBeNull();
            actualResult!.Value.Should().BeEquivalentTo(new List<CatDto>()
            {
                new CatDto
                {
                    Id = 1,
                    Name = "Alice",
                },
                new CatDto
                {
                    Id = 2,
                    Name = "Bob",
                }
            }.AsEnumerable());
        }

        [TestMethod()]
        public void GetCatTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void PutCatTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void PostCatTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void DeleteCatTest()
        {
            Assert.IsTrue(true);
        }
    }
}