
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pleromi.DAL.DatabaseContext;
using Pleromi.DAL.Entities;

namespace Pleromi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPackagesController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public SubscriptionPackagesController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/SubscriptionPackages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionPackage>>> GetSubscriptionPackages()
        {
            return await _context.SubscriptionPackages.ToListAsync();
        }

        // GET: api/SubscriptionPackages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionPackage>> GetSubscriptionPackage(int id)
        {
            var subscriptionPackage = await _context.SubscriptionPackages.FindAsync(id);

            if (subscriptionPackage == null)
            {
                return NotFound();
            }

            return subscriptionPackage;
        }

        // POST: api/SubscriptionPackages
        [HttpPost]
        public async Task<ActionResult<SubscriptionPackage>> PostSubscriptionPackage(SubscriptionPackage subscriptionPackage)
        {
            _context.SubscriptionPackages.Add(subscriptionPackage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubscriptionPackage), new { id = subscriptionPackage.SubscriptionPackageID }, subscriptionPackage);
        }

        // PUT: api/SubscriptionPackages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscriptionPackage(int id, [FromBody] SubscriptionPackage updatedSubscriptionPackage)
        {
            if (id != updatedSubscriptionPackage.SubscriptionPackageID)
            {
                return BadRequest();
            }

            var subscriptionPackage = await _context.SubscriptionPackages.FindAsync(id);
            if (subscriptionPackage == null)
            {
                return NotFound();
            }

            // Update the properties of the existing subscriptionPackage entity with the values from the updatedSubscriptionPackage object
            subscriptionPackage.NameEn = updatedSubscriptionPackage.NameEn;
            subscriptionPackage.NameAr = updatedSubscriptionPackage.NameAr;
            subscriptionPackage.NameUr = updatedSubscriptionPackage.NameUr;
            subscriptionPackage.DescriptionEn = updatedSubscriptionPackage.DescriptionEn;
            subscriptionPackage.DescriptionAr = updatedSubscriptionPackage.DescriptionAr;
            subscriptionPackage.DescriptionUr = updatedSubscriptionPackage.DescriptionUr;
            subscriptionPackage.Amount = updatedSubscriptionPackage.Amount;
            subscriptionPackage.IsActive = updatedSubscriptionPackage.IsActive;
            subscriptionPackage.CreatedOn = updatedSubscriptionPackage.CreatedOn;
            subscriptionPackage.CreatedBy = updatedSubscriptionPackage.CreatedBy;
            subscriptionPackage.ModifiedOn = updatedSubscriptionPackage.ModifiedOn;
            subscriptionPackage.ModifiedBy = updatedSubscriptionPackage.ModifiedBy;

            _context.Entry(subscriptionPackage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionPackageExists(id))
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

        // PATCH: api/SubscriptionPackages/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSubscriptionPackage(int id, [FromBody] JsonPatchDocument<SubscriptionPackage> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var subscriptionPackageToUpdate = await _context.SubscriptionPackages.FindAsync(id);
            if (subscriptionPackageToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the subscriptionPackage entity
                patchDocument.ApplyTo(subscriptionPackageToUpdate,
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

        // DELETE: api/SubscriptionPackages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriptionPackage(int id)
        {
            var subscriptionPackage = await _context.SubscriptionPackages.FindAsync(id);
            if (subscriptionPackage == null)
            {
                return NotFound();
            }

            _context.SubscriptionPackages.Remove(subscriptionPackage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionPackageExists(int id)
        {
            return _context.SubscriptionPackages.Any(e => e.SubscriptionPackageID == id);
        }
    }
}
