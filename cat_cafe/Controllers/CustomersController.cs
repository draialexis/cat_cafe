using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cat_cafe.Entities;
using cat_cafe.Repositories;
using cat_cafe.Dto;
using AutoMapper;
using Serilog;
using Newtonsoft.Json;

namespace cat_cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(CustomerContext context,IMapper mapper,ILogger<CustomersController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Customers
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

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(long id)
        {
            Log.Information(this.Request.Method + " => get by ID {@id}",id);
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
            return _mapper.Map<CustomerDto>(customer);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(long id, CustomerDto customerDto)
        {
            Log.Information(this.Request.Method + " => put by ID {@id}", id);
            if (id != customerDto.Id)
            {
                Log.Information(this.Request.Method + " => " + BadRequest().StatusCode.ToString()+" IDs not matching");
                return BadRequest();
            }

            Customer customer = _mapper.Map<Customer>(customerDto);

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!CustomerExists(id))
                {
                    Log.Information(this.Request.Method + " => " + NotFound().StatusCode.ToString());
                    return NotFound();
                }
                else
                {
                    Log.Error(this.Request.Method + " => " + e.Message);
                    throw;
                }
            }
            Log.Information(this.Request.Method + " => "
                + this.Response.StatusCode.ToString() + " "
                + customer.GetType().ToString() + " "
                + JsonConvert.SerializeObject(customer).ToString());
            return Ok();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerDto customerDto)
        {
            Log.Information(this.Request.Method + " => post customer");

            Customer customer = _mapper.Map<Customer>(customerDto);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            Log.Information(this.Request.Method + " => "
                + 201 + " "
                + customer.GetType().ToString() + " "
                + JsonConvert.SerializeObject(customer).ToString());

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, _mapper.Map<Customer>( customer));
        }

        // DELETE: api/Customers/5
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

        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
