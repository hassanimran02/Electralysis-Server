
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
    public class SubscriptionFeaturesController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public SubscriptionFeaturesController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/SubscriptionFeatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionFeature>>> GetSubscriptionFeatures()
        {
            return await _context.SubscriptionFeatures.ToListAsync();
        }

        // GET: api/SubscriptionFeatures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionFeature>> GetSubscriptionFeature(int id)
        {
            var subscriptionFeature = await _context.SubscriptionFeatures.FindAsync(id);

            if (subscriptionFeature == null)
            {
                return NotFound();
            }

            return subscriptionFeature;
        }

        // POST: api/SubscriptionFeatures
        [HttpPost]
        public async Task<ActionResult<SubscriptionFeature>> PostSubscriptionFeature(SubscriptionFeature subscriptionFeature)
        {
            _context.SubscriptionFeatures.Add(subscriptionFeature);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubscriptionFeature), new { id = subscriptionFeature.SubscriptionPackageFeatureID }, subscriptionFeature);
        }

        // PUT: api/SubscriptionFeatures/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscriptionFeature(int id, [FromBody] SubscriptionFeature updatedSubscriptionFeature)
        {
            if (id != updatedSubscriptionFeature.SubscriptionPackageFeatureID)
            {
                return BadRequest();
            }

            var subscriptionFeature = await _context.SubscriptionFeatures.FindAsync(id);
            if (subscriptionFeature == null)
            {
                return NotFound();
            }

            // Update the properties of the existing subscriptionFeature entity with the values from the updatedSubscriptionFeature object
            subscriptionFeature.SubscriptionPackageID = updatedSubscriptionFeature.SubscriptionPackageID;
            subscriptionFeature.FeatureEn = updatedSubscriptionFeature.FeatureEn;
            subscriptionFeature.FeatureAr = updatedSubscriptionFeature.FeatureAr;
            subscriptionFeature.FeatureUr = updatedSubscriptionFeature.FeatureUr;
            subscriptionFeature.IsAvailable = updatedSubscriptionFeature.IsAvailable;
            subscriptionFeature.IsActive = updatedSubscriptionFeature.IsActive;
            subscriptionFeature.CreatedOn = updatedSubscriptionFeature.CreatedOn;
            subscriptionFeature.CreatedBy = updatedSubscriptionFeature.CreatedBy;
            subscriptionFeature.ModifiedOn = updatedSubscriptionFeature.ModifiedOn;
            subscriptionFeature.ModifiedBy = updatedSubscriptionFeature.ModifiedBy;

            _context.Entry(subscriptionFeature).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionFeatureExists(id))
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

        // PATCH: api/SubscriptionFeatures/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSubscriptionFeature(int id, [FromBody] JsonPatchDocument<SubscriptionFeature> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var subscriptionFeatureToUpdate = await _context.SubscriptionFeatures.FindAsync(id);
            if (subscriptionFeatureToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the subscriptionFeature entity
                patchDocument.ApplyTo(subscriptionFeatureToUpdate,
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

        // DELETE: api/SubscriptionFeatures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriptionFeature(int id)
        {
            var subscriptionFeature = await _context.SubscriptionFeatures.FindAsync(id);
            if (subscriptionFeature == null)
            {
                return NotFound();
            }

            _context.SubscriptionFeatures.Remove(subscriptionFeature);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionFeatureExists(int id)
        {
            return _context.SubscriptionFeatures.Any(e => e.SubscriptionPackageFeatureID == id);
        }
    }
}
