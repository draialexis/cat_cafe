using System;
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

namespace cat_cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private readonly CatCafeDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CatsController> _logger;

        public CatsController(CatCafeDbContext context, IMapper mapper, ILogger<CatsController> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        // GET: api/Cats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatDto>>> GetCats()
        {
            var cats = await _context.cats.ToListAsync();
            return _mapper.Map<List<CatDto>>(cats);

            return Ok(_mapper.Map<List<CatDto>>(cats));
        }

        // GET: api/Cats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CatDto>> GetCat(long id)
        {
            var cat = await _context.cats.FindAsync(id);

            if (cat == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CatDto>(cat));
        }

        // PUT: api/Cats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

        // POST: api/Cats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CatDto>> PostCat(CatDto catDto)
        {
            Cat cat = _mapper.Map<Cat>(catDto);
            _context.cats.Add(cat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCat", new { id = catDto.Id }, _mapper.Map<CatDto>(cat));
        }

        // DELETE: api/Cats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCat(long id)
        {
            var cat = await _context.cats.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }

            _context.cats.Remove(cat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CatExists(long id)
        {
            return _context.cats.Any(e => e.Id == id);
        }
    }
}
