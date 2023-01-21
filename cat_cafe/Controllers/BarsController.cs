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

namespace cat_cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarsController : ControllerBase
    {
        private readonly BarContext _context;
        private readonly IMapper _mapper;

        public BarsController(BarContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Bars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BarDto>>> GetBars()
        {
            var bars = await _context.Bars.ToListAsync();
            return _mapper.Map<List<BarDto>>(bars);
        }

        // GET: api/Bars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BarDto>> GetBar(long id)
        {
            var bar = await _context.Bars.FindAsync(id);

            if (bar == null)
            {
                return NotFound();
            }

            return _mapper.Map<BarDto>(bar);
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
            Bar bar = _mapper.Map<Bar>(barDto);
            _context.Bars.Add(bar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBar", new { id = barDto.Id }, _mapper.Map<BarDto>(bar));
        }

        // DELETE: api/Bars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBar(long id)
        {
            var bar = await _context.Bars.FindAsync(id);
            if (bar == null)
            {
                return NotFound();
            }

            _context.Bars.Remove(bar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BarExists(long id)
        {
            return _context.Bars.Any(e => e.Id == id);
        }
    }
}
