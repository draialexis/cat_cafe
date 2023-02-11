﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cat_cafe.Entities;
using cat_cafe.Repositories;
using AutoMapper;
using cat_cafe.Dto;
using Serilog;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging.Abstractions;
using cat_cafe.WeSo;

namespace cat_cafe.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CatsController : ControllerBase
    {
        private readonly CatCafeContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CatsController> _logger;
        private readonly WebSocketHandler _webSocketHandler;

        public CatsController(
            CatCafeContext context, 
            IMapper mapper, 
            ILogger<CatsController> logger,
            WebSocketHandler webSocketHandler
            )
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
            _webSocketHandler = webSocketHandler;
        }

        // GET: api/v1/Cats
        [HttpGet]
        [MapToApiVersion("1.0")]

        public async Task<ActionResult<IEnumerable<CatDto>>> GetCats()
        {
            var cats = await _context.Cats.ToListAsync();
            cats.Add(new Cat { Id = -1, Age = 42, Name = "Hi! I'm the secret V1 cat" });
            return Ok(_mapper.Map<List<CatDto>>(cats));
        }


        // GET: api/v2/Cats
        [HttpGet]
        [MapToApiVersion("2.0")]

        public async Task<ActionResult<IEnumerable<CatDto>>> GetCatsV2()
        {
            var cats = await _context.Cats.ToListAsync();

            return Ok(_mapper.Map<List<CatDto>>(cats));
        }

        // GET: api/v1/Cats/5
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<CatDto>> GetCat(long id)
        {
            var cat = await _context.Cats.FindAsync(id);

            if (cat == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CatDto>(cat));
        }

        // PUT: api/v1/Cats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> PutCat(long id, CatDto catDto)
        {
            if (id != catDto.Id)
            {
                return BadRequest();
            }

            Cat cat = _mapper.Map<Cat>(catDto);
            _context.Entry(cat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/v1/Cats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<CatDto>> PostCat(CatDto catDto)
        {
            Cat cat = _mapper.Map<Cat>(catDto);
            _context.Cats.Add(cat);
            await _context.SaveChangesAsync();

            await _webSocketHandler.BroadcastMessageAsync("entity-created");

            return CreatedAtAction("GetCat", new { id = catDto.Id }, _mapper.Map<CatDto>(cat));
        }

        // DELETE: api/v1/Cats/5
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteCat(long id)
        {
            var cat = await _context.Cats.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }

            _context.Cats.Remove(cat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CatExists(long id)
        {
            return _context.Cats.Any(e => e.Id == id);
        }
    }
}
