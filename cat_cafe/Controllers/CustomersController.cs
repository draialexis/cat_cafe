using AutoMapper;
using cat_cafe.Dto;
using cat_cafe.Entities;
using cat_cafe.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace cat_cafe.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CustomersController : ControllerBase
    {
        private readonly CatCafeContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(CatCafeContext context, IMapper mapper, ILogger<CustomersController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/v1/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();
                _logger.LogInformation("Customers retrieved successfully.");
                return Ok(_mapper.Map<List<CustomerDto>>(customers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve customers.");
                return BadRequest(ex);
            }
        }

        // GET: api/v1/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(long id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    _logger.LogInformation("Customer not found.");
                    return NotFound();
                }

                _logger.LogInformation("Customer retrieved successfully.");
                return Ok(_mapper.Map<CustomerDto>(customer));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve customer.");
                return BadRequest(ex);
            }
        }

        // PUT: api/v1/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(long id, CustomerDto customerDto)
        {
            try
            {
                if (id != customerDto.Id)
                {
                    _logger.LogError("No such customer.");
                    return BadRequest();
                }

                var customer = await _context.Customers
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (customer == null)
                {
                    _logger.LogInformation("Customer not found.");
                    return NotFound();
                }

                _mapper.Map(customerDto, customer);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Customer updated successfully.");
                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update customer.");
                return BadRequest(ex);
            }
        }

        // POST: api/v1/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customerDto)
        {
            try
            {
                Customer customer = _mapper.Map<Customer>(customerDto);
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Customer created successfully.");
                return CreatedAtAction("GetCustomer", new { id = customer.Id }, _mapper.Map<CustomerDto>(customer));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create customer.");
                return BadRequest(ex);
            }

        }

        // DELETE: api/v1/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Customer deleted successfully.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete customer.");
                return BadRequest(ex);
            }
        }
    }
}
