
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
    public class UserSubscriptionsController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public UserSubscriptionsController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/UserSubscriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSubscription>>> GetUserSubscriptions()
        {
            return await _context.UserSubscriptions.ToListAsync();
        }

        // GET: api/UserSubscriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserSubscription>> GetUserSubscription(int id)
        {
            var userSubscription = await _context.UserSubscriptions.FindAsync(id);

            if (userSubscription == null)
            {
                return NotFound();
            }

            return userSubscription;
        }

        // POST: api/UserSubscriptions
        [HttpPost]
        public async Task<ActionResult<UserSubscription>> PostUserSubscription(UserSubscription userSubscription)
        {
            _context.UserSubscriptions.Add(userSubscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserSubscription), new { id = userSubscription.UserSubscriptionID }, userSubscription);
        }

        // PUT: api/UserSubscriptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserSubscription(int id, [FromBody] UserSubscription updatedUserSubscription)
        {
            if (id != updatedUserSubscription.UserSubscriptionID)
            {
                return BadRequest();
            }

            var userSubscription = await _context.UserSubscriptions.FindAsync(id);
            if (userSubscription == null)
            {
                return NotFound();
            }

            // Update the properties of the existing userSubscription entity with the values from the updatedUserSubscription object
            userSubscription.SubscriptionRequestID = updatedUserSubscription.SubscriptionRequestID;
            userSubscription.IsActivated = updatedUserSubscription.IsActivated;
            userSubscription.StartedOn = updatedUserSubscription.StartedOn;
            userSubscription.EndedOn = updatedUserSubscription.EndedOn;
            userSubscription.IsRenewed = updatedUserSubscription.IsRenewed;
            userSubscription.IsActive = updatedUserSubscription.IsActive;
            userSubscription.CreatedOn = updatedUserSubscription.CreatedOn;
            userSubscription.CreatedBy = updatedUserSubscription.CreatedBy;
            userSubscription.ModifiedOn = updatedUserSubscription.ModifiedOn;

            _context.Entry(userSubscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserSubscriptionExists(id))
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

        // PATCH: api/UserSubscriptions/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUserSubscription(int id, [FromBody] JsonPatchDocument<UserSubscription> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var userSubscriptionToUpdate = await _context.UserSubscriptions.FindAsync(id);
            if (userSubscriptionToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the userSubscription entity
                patchDocument.ApplyTo(userSubscriptionToUpdate,
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

        // DELETE: api/UserSubscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserSubscription(int id)
        {
            var userSubscription = await _context.UserSubscriptions.FindAsync(id);
            if (userSubscription == null)
            {
                return NotFound();
            }

            _context.UserSubscriptions.Remove(userSubscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserSubscriptionExists(int id)
        {
            return _context.UserSubscriptions.Any(e => e.UserSubscriptionID == id);
        }
    }
}
