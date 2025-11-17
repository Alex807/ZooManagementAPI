using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Enums;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ZooManagementDbContext _context;

        public RoleController(ZooManagementDbContext context)
        {
            _context = context;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var roles = await _context.Roles
                .AsNoTracking()
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                })
                .ToListAsync();

            return Ok(roles);
        }

        // GET: api/Role/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            var role = await _context.Roles
                .AsNoTracking()
                .Where(r => r.Id == id)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                })
                .FirstOrDefaultAsync();

            if (role == null) return NotFound();
            return Ok(role);
        }

        // POST: api/Role
        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole(CreateRoleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var exists = await _context.Roles.AnyAsync(r => r.Name == dto.Name);
            if (exists) return Conflict(new { message = "A role with this name already exists." });

            var role = new Role
            {
                Name = dto.Name,  
                Description = dto.Description ?? string.Empty
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            var result = new RoleDto { Id = role.Id, Name = role.Name, Description = role.Description };
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, result);
        }

        // PUT: api/Role/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRole(int id, UpdateRoleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest(new { message = "Id in path and body must match." });

            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            // Optional: prevent changing name to an existing name
            if (role.Name != dto.Name)
            {
                var exists = await _context.Roles.AnyAsync(r => r.Id != id && r.Name == dto.Name);
                if (exists) return Conflict(new { message = "A role with this name already exists." });
            }

            role.Name = dto.Name;
            role.Description = dto.Description ?? string.Empty;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Roles.AnyAsync(r => r.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Role/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DTOs
        public class RoleDto
        {
            public int Id { get; set; }
            public RoleName Name { get; set; }
            public string Description { get; set; } = string.Empty;
        }

        public class CreateRoleDto
        {
            public RoleName Name { get; set; }
            public string? Description { get; set; }
        }

        public class UpdateRoleDto
        {
            public int Id { get; set; }
            public RoleName Name { get; set; }
            public string? Description { get; set; }
        }
    }
}

