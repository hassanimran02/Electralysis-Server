
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
    public class SubscriptionRequestsController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public SubscriptionRequestsController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/SubscriptionRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionRequest>>> GetSubscriptionRequests()
        {
            return await _context.SubscriptionRequests.ToListAsync();
        }

        // GET: api/SubscriptionRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionRequest>> GetSubscriptionRequest(int id)
        {
            var subscriptionRequest = await _context.SubscriptionRequests.FindAsync(id);

            if (subscriptionRequest == null)
            {
                return NotFound();
            }

            return subscriptionRequest;
        }

        // POST: api/SubscriptionRequests
        [HttpPost]
        public async Task<ActionResult<SubscriptionRequest>> PostSubscriptionRequest(SubscriptionRequest subscriptionRequest)
        {
            _context.SubscriptionRequests.Add(subscriptionRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubscriptionRequest), new { id = subscriptionRequest.SubscriptionRequestID }, subscriptionRequest);
        }

        // PUT: api/SubscriptionRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscriptionRequest(int id, [FromBody] SubscriptionRequest updatedSubscriptionRequest)
        {
            if (id != updatedSubscriptionRequest.SubscriptionRequestID)
            {
                return BadRequest();
            }

            var subscriptionRequest = await _context.SubscriptionRequests.FindAsync(id);
            if (subscriptionRequest == null)
            {
                return NotFound();
            }

            // Update the properties of the existing subscriptionRequest entity with the values from the updatedSubscriptionRequest object
            subscriptionRequest.SubscriptionPackageID = updatedSubscriptionRequest.SubscriptionPackageID;
            subscriptionRequest.UserID = updatedSubscriptionRequest.UserID;
            subscriptionRequest.StatusID = updatedSubscriptionRequest.StatusID;
            subscriptionRequest.PaymentTypeID = updatedSubscriptionRequest.PaymentTypeID;
            subscriptionRequest.CreatedOn = updatedSubscriptionRequest.CreatedOn;
            subscriptionRequest.CreatedBy = updatedSubscriptionRequest.CreatedBy;
            subscriptionRequest.ModifiedOn = updatedSubscriptionRequest.ModifiedOn;
            subscriptionRequest.ModifiedBy = updatedSubscriptionRequest.ModifiedBy;

            _context.Entry(subscriptionRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionRequestExists(id))
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

        // PATCH: api/SubscriptionRequests/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSubscriptionRequest(int id, [FromBody] JsonPatchDocument<SubscriptionRequest> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var subscriptionRequestToUpdate = await _context.SubscriptionRequests.FindAsync(id);
            if (subscriptionRequestToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the subscriptionRequest entity
                patchDocument.ApplyTo(subscriptionRequestToUpdate,
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

        // DELETE: api/SubscriptionRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriptionRequest(int id)
        {
            var subscriptionRequest = await _context.SubscriptionRequests.FindAsync(id);
            if (subscriptionRequest == null)
            {
                return NotFound();
            }

            _context.SubscriptionRequests.Remove(subscriptionRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionRequestExists(int id)
        {
            return _context.SubscriptionRequests.Any(e => e.SubscriptionRequestID == id);
        }
    }
}
