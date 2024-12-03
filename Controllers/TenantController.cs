using BasicSaasTenent.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasicSaasTenent.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TenantController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllTenants() {
            var tenants = _context.Tenants.ToList();
            return Ok(tenants);
        }

        [HttpGet("{id}")]
        public IActionResult GetTenant(string id) {
            var tenant = _context.Tenants.Find(id);
            if (tenant == null) {
                return NotFound();
            }
            return Ok(tenant);
        }

        [HttpPost]
        public IActionResult CreateTenant([FromBody] Tenant tenant) { 
            _context.Tenants.Add(tenant);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, tenant);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateTenant(string id, [FromBody] Tenant tenant)
        {
            var existingTenant = _context.Tenants.Find(id);
            if (existingTenant == null) return NotFound();

            existingTenant.Name = tenant.Name;
            existingTenant.IsActive = tenant.IsActive;
            existingTenant.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTenant(string id)
        {
            var tenant = _context.Tenants.Find(id);
            if (tenant == null) return NotFound();

            _context.Tenants.Remove(tenant);
            _context.SaveChanges();
            return NoContent();
        }


    }
}
