using AutoMapper;
using cat_cafe.Dto;
using cat_cafe.Entities;
using cat_cafe.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cat_cafe.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BarsController : ControllerBase
    {
        private readonly CatCafeContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BarsController> _logger;

        public BarsController(CatCafeContext context, IMapper mapper, ILogger<BarsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/v1/Bars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BarDto>>> GetBars()
        {
            var bars = await _context.Bars
               .Include(b => b.Cats)
               .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<BarDto>>(bars));
        }

        // GET: api/v1/Bars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BarDto>> GetBar(long id)
        {
            var bar = await _context.Bars
                .Include(b => b.Cats)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (bar == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BarDto>(bar));
        }

        // PUT: api/v1/Bars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBar(long id, BarDto barDto)
        {
            if (id != barDto.Id)
            {
                return BadRequest();
            }

            var bar = await _context.Bars
                .Include(b => b.Cats)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (bar == null)
            {
                return NotFound();
            }

            _mapper.Map(barDto, bar);
            bar.Cats = await _context.Cats
                .Where(c => barDto.CatIds.Contains(c.Id))
                .ToListAsync();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/v1/Bars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BarDto>> CreateBar(BarDto barDto)
        {
            var bar = _mapper.Map<Bar>(barDto);
            bar.Cats = await _context.Cats
                .Where(c => barDto.CatIds.Contains(c.Id))
                .ToListAsync();

            _context.Bars.Add(bar);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBar), new { id = bar.Id }, _mapper.Map<BarDto>(bar));
        }

        // DELETE: api/v1/Bars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBar(long id)
        {
            var bar = await _context.Bars
                .Include(b => b.Cats)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (bar == null)
            {
                return NotFound();
            }

            _context.Bars.Remove(bar);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
