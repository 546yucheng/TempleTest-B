using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aprojectbackend.Models;
using Microsoft.AspNetCore.Cors;

namespace Aprojectbackend.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("All")]
    public class TUserSignupController : ControllerBase
    {
        private readonly AprojectContext _context;

        public TUserSignupController(AprojectContext context)
        {
            _context = context;
        }

        // GET: api/TUserSignup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TUser>>> GetTUsers()
        {
            return await _context.TUsers.ToListAsync();
        }

        // GET: api/TUserSignup/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TUser>> GetTUser(int id)
        {
            var tUser = await _context.TUsers.FindAsync(id);

            if (tUser == null)
            {
                return NotFound();
            }

            return tUser;
        }

        // PUT: api/TUserSignup/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTUser(int id, TUser tUser)
        {
            if (id != tUser.FUserId)
            {
                return BadRequest();
            }

            _context.Entry(tUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TUserExists(id))
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

        // POST: api/TUserSignup
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TUser>> PostTUser(TUser tUser)
        {
            _context.TUsers.Add(tUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTUser", new { id = tUser.FUserId }, tUser);
        }


        // DELETE: api/TUserSignup/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTUser(int id)
        {
            var tUser = await _context.TUsers.FindAsync(id);
            if (tUser == null)
            {
                return NotFound();
            }

            _context.TUsers.Remove(tUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TUserExists(int id)
        {
            return _context.TUsers.Any(e => e.FUserId == id);
        }
    }
}
