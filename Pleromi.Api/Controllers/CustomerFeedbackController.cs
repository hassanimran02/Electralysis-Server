
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
    public class CustomerFeedbackController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public CustomerFeedbackController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomerFeedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerFeedback>>> GetCustomerFeedback()
        {
            return await _context.CustomerFeedbacks.ToListAsync();
        }

        // GET: api/CustomerFeedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerFeedback>> GetCustomerFeedback(int id)
        {
            var feedback = await _context.CustomerFeedbacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }

        // POST: api/CustomerFeedback
        [HttpPost]
        public async Task<ActionResult<CustomerFeedback>> PostCustomerFeedback(CustomerFeedback feedback)
        {
            _context.CustomerFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerFeedback), new { id = feedback.CustomerFeedbackID }, feedback);
        }

        // PUT: api/CustomerFeedback/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerFeedback(int id, [FromBody] CustomerFeedback updatedFeedback)
        {
            if (id != updatedFeedback.CustomerFeedbackID)
            {
                return BadRequest();
            }

            var feedback = await _context.CustomerFeedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            // Update the properties of the existing feedback entity with the values from the updatedFeedback object
            feedback.FullName = updatedFeedback.FullName;
            feedback.Email = updatedFeedback.Email;
            feedback.MobileNo = updatedFeedback.MobileNo;
            feedback.Subject = updatedFeedback.Subject;
            feedback.Message = updatedFeedback.Message;
            feedback.AttachmentUrl = updatedFeedback.AttachmentUrl;
            feedback.ModifiedOn = DateTime.UtcNow; // Assuming modification timestamp is updated on PUT
            feedback.ModifiedBy = updatedFeedback.ModifiedBy;

            _context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
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

        // PATCH: api/CustomerFeedback/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCustomerFeedback(int id, [FromBody] JsonPatchDocument<CustomerFeedback> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var feedbackToUpdate = await _context.CustomerFeedbacks.FindAsync(id);
            if (feedbackToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the feedback entity
                patchDocument.ApplyTo(feedbackToUpdate,
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

        // DELETE: api/CustomerFeedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerFeedback(int id)
        {
            var feedback = await _context.CustomerFeedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.CustomerFeedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeedbackExists(int id)
        {
            return _context.CustomerFeedbacks.Any(e => e.CustomerFeedbackID == id);
        }
    }
}
