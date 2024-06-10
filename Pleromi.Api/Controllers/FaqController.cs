
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
    public class FaqController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public FaqController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/Faq
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faq>>> GetFaqs()
        {
            return await _context.Faqs.ToListAsync();
        }

        // GET: api/Faq/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> GetFaq(int id)
        {
            var faq = await _context.Faqs.FindAsync(id);

            if (faq == null)
            {
                return NotFound();
            }

            return faq;
        }

        // POST: api/Faq
        [HttpPost]
        public async Task<ActionResult<Faq>> PostFaq(Faq faq)
        {
            _context.Faqs.Add(faq);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFaq), new { id = faq.FaqID }, faq);
        }

        // PUT: api/Faq/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFaq(int id, [FromBody] Faq updatedFaq)
        {
            if (id != updatedFaq.FaqID)
            {
                return BadRequest();
            }

            var faq = await _context.Faqs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }

            // Update the properties of the existing faq entity with the values from the updatedFaq object
            faq.QuestionEn = updatedFaq.QuestionEn;
            faq.AnswerEn = updatedFaq.AnswerEn;
            faq.QuestionAr = updatedFaq.QuestionAr;
            faq.AnswerAr = updatedFaq.AnswerAr;
            faq.QuestionUr = updatedFaq.QuestionUr;
            faq.AnswerUr = updatedFaq.AnswerUr;
            faq.IsActive = updatedFaq.IsActive;

            _context.Entry(faq).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaqExists(id))
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

        // PATCH: api/Faq/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchFaq(int id, [FromBody] JsonPatchDocument<Faq> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var faqToUpdate = await _context.Faqs.FindAsync(id);
            if (faqToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the faq entity
                patchDocument.ApplyTo(faqToUpdate,
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

        // DELETE: api/Faq/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaq(int id)
        {
            var faq = await _context.Faqs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }

            _context.Faqs.Remove(faq);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FaqExists(int id)
        {
            return _context.Faqs.Any(e => e.FaqID == id);
        }
    }
}
