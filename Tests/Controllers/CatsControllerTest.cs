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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cat_cafe.Controllers.Tests
{

    [TestClass()]
    public class CatsControllerTest
    {

        private readonly ILogger<CatsController> logger = new NullLogger<CatsController>();

        private readonly MapperConfiguration mapperConf = new(mapper => mapper.AddProfile(typeof(CatMapper)));

        private readonly IMapper mapper;

        private readonly DbContextOptions<CatContext> options = new DbContextOptionsBuilder<CatContext>()
                .UseInMemoryDatabase(databaseName: "CatCafeTests")
                .Options;

        private readonly CatContext context;

        private readonly CatsController controller;

        public CatsControllerTest()
        {
            mapper = mapperConf.CreateMapper();
            context = new CatContext(options);
            controller = new CatsController(context, mapper, logger);
        }


        [TestInitialize]
        public void StartUp()
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

            // TODO tear down and drop all before each test method
        }

        [TestMethod()]
        public async Task GetCatsTest()
        {
            ActionResult<IEnumerable<CatDto>> actual = await controller.GetCats();
            Assert.Equals(200, actual.Result);
        }

        [TestMethod()]
        public void GetCatTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutCatTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostCatTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteCatTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CatExistsTest()
        {
            Assert.Fail();
        }

    }
}