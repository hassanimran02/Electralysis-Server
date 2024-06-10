
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pleromi.DAL.DatabaseContext;
using Pleromi.DAL.Entities;

namespace Pleromi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerTypesController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public ConsumerTypesController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/ConsumerTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsumerType>>> GetConsumerTypes()
        {
            return await _context.ConsumerTypes.ToListAsync();
        }

        // GET: api/ConsumerTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConsumerType>> GetConsumerType(int id)
        {
            var consumerType = await _context.ConsumerTypes.FindAsync(id);

            if (consumerType == null)
            {
                return NotFound();
            }

            return consumerType;
        }

        // POST: api/ConsumerTypes
        [HttpPost]
        public async Task<ActionResult<ConsumerType>> PostConsumerType(ConsumerType consumerType)
        {
            _context.ConsumerTypes.Add(consumerType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsumerType), new { id = consumerType.ConsumerTypeID }, consumerType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsumerType(int id, ConsumerType updatedConsumerType)
        {
            if (id != updatedConsumerType.ConsumerTypeID)
            {
                return BadRequest();
            }

            var consumerType = await _context.ConsumerTypes.FindAsync(id);
            if (consumerType == null)
            {
                return NotFound();
            }

            // Update the properties of the existing consumer type entity with the values from the updatedConsumerType object
            consumerType.NameEn = updatedConsumerType.NameEn;
            consumerType.NameAr = updatedConsumerType.NameAr;
            consumerType.NameUr = updatedConsumerType.NameUr;
            consumerType.IsActive = updatedConsumerType.IsActive;

            _context.Entry(consumerType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsumerTypeExists(id))
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

        // PATCH: api/ConsumerTypes/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchConsumerType(int id, [FromBody] JsonPatchDocument<ConsumerType> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var consumerTypeToUpdate = await _context.ConsumerTypes.FindAsync(id);
            if (consumerTypeToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the consumerType entity
                patchDocument.ApplyTo(consumerTypeToUpdate,
                    (Microsoft.AspNetCore.JsonPatch.JsonPatchError err) => ModelState.AddModelError("JsonPatch", err.ErrorMessage));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        // DELETE: api/ConsumerTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsumerType(int id)
        {
            var consumerType = await _context.ConsumerTypes.FindAsync(id);
            if (consumerType == null)
            {
                return NotFound();
            }

            _context.ConsumerTypes.Remove(consumerType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConsumerTypeExists(int id)
        {
            return _context.ConsumerTypes.Any(e => e.ConsumerTypeID == id);
        }
    }
}
