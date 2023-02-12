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
            Log.Information(this.Request.Method + " => get All customers");

            var customers = await _context.Customers.ToListAsync();

            Log.Information(this.Request.Method + " => "
                + this.Response.StatusCode.ToString() + " "
                + customers.GetType().ToString() + " length["
                + customers.Count + "]");
            return Ok(_mapper.Map<List<CustomerDto>>(customers));
        }

        // GET: api/v1/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(long id)
        {
            Log.Information(this.Request.Method + " => get by ID {@id}", id);
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {

                Log.Information(this.Request.Method + " => " + NotFound().StatusCode.ToString());
                return NotFound();
            }
            Log.Information(this.Request.Method + " => "
                + this.Response.StatusCode.ToString() + " "
                + customer.GetType().ToString() + " "
                + JsonConvert.SerializeObject(customer).ToString());
            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        // PUT: api/v1/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(long id, CustomerDto customerDto)
        {
            Log.Information(this.Request.Method + " => put by ID {@id}", id);
            if (id != customerDto.Id)
            {
                Log.Information(this.Request.Method + " => " + BadRequest().StatusCode.ToString() + " IDs not matching");
                return BadRequest();
            }

            var customer = await _context.Customers
                .SingleOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            _mapper.Map(customerDto, customer);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/v1/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customerDto)
        {
            Log.Information(this.Request.Method + " => post customer");

            Customer customer = _mapper.Map<Customer>(customerDto);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            Log.Information(this.Request.Method + " => "
                + 201 + " "
                + customer.GetType().ToString() + " "
                + JsonConvert.SerializeObject(customer).ToString());

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, _mapper.Map<CustomerDto>(customer));
        }

        // DELETE: api/v1/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            Log.Information(this.Request.Method + " => delete by ID {@id}", id);

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                Log.Information(this.Request.Method + " => " + NotFound().StatusCode.ToString());
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            Log.Information(this.Request.Method + " => "
                + this.Response.StatusCode.ToString() + " "
                + customer.GetType().ToString() + " "
                + JsonConvert.SerializeObject(customer).ToString());

            return Ok();
        }
    }
}
