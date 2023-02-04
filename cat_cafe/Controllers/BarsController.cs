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
using System.Collections;
using System.Xml.Linq;

namespace cat_cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarsController : ControllerBase
    {
        private readonly CatCafeDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger <BarsController> _logger;

        public BarsController(CatCafeDbContext context,IMapper mapper, ILogger<BarsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Bars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BarDto>>> GetBars()
        {
            //return await _context.bars.ToListAsync();
            var bars = _context.bars
               .Include(a => a.cats)
               .Select(a => new Bar
               {
                   Id = a.Id,
                   Name = a.Name,
                  cats = a.cats.Select(p => new Cat { Name = p.Name, Age = p.Age, Id= p.Id}).ToList()
               })
                .ToListAsync();
            return _mapper.Map<List<BarDto>>(bars);
        }

        // GET: api/Bars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BarDto>> GetBar(long id)
        {
            //var bar = await _context.bars.FindAsync(id);
            var bar = _context.bars.Include(p => p.cats)
                .Select(a => new Bar
                {
                    Id = a.Id,
                    Name = a.Name,
                    cats = a.cats.Select(p => new Cat { Name = p.Name, Age = p.Age, Id = p.Id }).ToList()

                }).FirstOrDefaultAsync(p => p.Id == id);

            if (bar == null)
            {
                return NotFound();
            }

            return _mapper.Map<BarDto>(bar.Result);
        }

        // PUT: api/Bars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBar(long id, BarDto barDto)
        {
            if (id != barDto.Id)
            {
                return BadRequest();
            }
            Bar bar = _mapper.Map<Bar>(barDto);
            _context.Entry(bar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BarExists(id))
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

        // POST: api/Bars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BarDto>> PostBar(BarDto barDto)
        {
            _context.bars.Add(_mapper.Map<Bar>(barDto)) ;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBar", new { id = barDto.Id }, _mapper.Map<BarDto>(barDto));
        }

        // DELETE: api/Bars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBar(long id)
        {
            var bar = await _context.bars.FindAsync(id);
            if (bar == null)
            {
                return NotFound();
            }

            _context.bars.Remove(bar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BarExists(long id)
        {
            return _context.bars.Any(e => e.Id == id);
        }
    }
}
