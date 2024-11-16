using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aprojectbackend.Models;

namespace Aprojectbackend.Controllers.lightController
{
    [Route("api/[controller]")]
    [ApiController]
    public class lightsController : ControllerBase
    {
        private readonly AprojectContext _context;

        public lightsController(AprojectContext context)
        {
            _context = context;
        }

        // GET: api/lights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TLight>>> GetTLights()
        {
            return await _context.TLights.ToListAsync();
        }

        // GET: api/lights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TLight>> GetTLight(int id)
        {
            var tLight = await _context.TLights.FindAsync(id);

            if (tLight == null)
            {
                return NotFound();
            }

            return tLight;
        }

        // PUT: api/lights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTLight(int id, TLight tLight)
        {
            if (id != tLight.FLightId)
            {
                return BadRequest();
            }

            _context.Entry(tLight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TLightExists(id))
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

        // POST: api/lights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TLight>> PostTLight(TLight tLight)
        {
            _context.TLights.Add(tLight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTLight", new { id = tLight.FLightId }, tLight);
        }

        // DELETE: api/lights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTLight(int id)
        {
            var tLight = await _context.TLights.FindAsync(id);
            if (tLight == null)
            {
                return NotFound();
            }

            _context.TLights.Remove(tLight);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TLightExists(int id)
        {
            return _context.TLights.Any(e => e.FLightId == id);
        }
    }
}
