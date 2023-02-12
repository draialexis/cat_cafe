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
            try
            {
                var bars = await _context.Bars
                               .Include(b => b.Cats)
                               .ToListAsync();
                _logger.LogInformation("Bars retrieved successfully.");
                return Ok(_mapper.Map<IEnumerable<BarDto>>(bars));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all bars.");
                return BadRequest(ex);
            }

        }

        // GET: api/v1/Bars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BarDto>> GetBar(long id)
        {
            try
            {
                var bar = await _context.Bars
                                .Include(b => b.Cats)
                                .SingleOrDefaultAsync(b => b.Id == id);

                if (bar == null)
                {
                    _logger.LogError("No such bar.");
                    return NotFound();
                }

                _logger.LogInformation("Bar retrieved successfully.");
                return Ok(_mapper.Map<BarDto>(bar));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve bar.");
                return BadRequest(ex);
            }

        }

        // PUT: api/v1/Bars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBar(long id, BarDto barDto)
        {
            try
            {
                if (id != barDto.Id)
                {
                    _logger.LogError("No such bar.");
                    return BadRequest();
                }

                var bar = await _context.Bars
                    .Include(b => b.Cats)
                    .SingleOrDefaultAsync(b => b.Id == id);

                if (bar == null)
                {
                    _logger.LogInformation("Bar not found.");
                    return NotFound();
                }

                _mapper.Map(barDto, bar);
                bar.Cats = await _context.Cats
                    .Where(c => barDto.CatIds.Contains(c.Id))
                    .ToListAsync();

                await _context.SaveChangesAsync();
                _logger.LogInformation("Bar updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update bar.");
                return BadRequest(ex);
            }

        }

        // POST: api/v1/Bars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BarDto>> CreateBar(BarDto barDto)
        {
            try
            {
                var bar = _mapper.Map<Bar>(barDto);
                bar.Cats = await _context.Cats
                    .Where(c => barDto.CatIds.Contains(c.Id))
                    .ToListAsync();

                _context.Bars.Add(bar);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Bar created succesfully.");
                return CreatedAtAction(nameof(GetBar), new { id = bar.Id }, _mapper.Map<BarDto>(bar));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create bar.");
                return BadRequest(ex);
            }

        }

        // DELETE: api/v1/Bars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBar(long id)
        {
            try
            {
                var bar = await _context.Bars
                    .Include(b => b.Cats)
                    .SingleOrDefaultAsync(b => b.Id == id);

                if (bar != null)
                {
                    _context.Bars.Remove(bar);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Bar deleted succesfully.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete bar.");
                return BadRequest(ex);
            }
        }
    }
}
