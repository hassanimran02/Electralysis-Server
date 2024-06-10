
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
    public class LanguageController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public LanguageController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/Language
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Language>>> GetLanguages()
        {
            return await _context.Languages.ToListAsync();
        }

        // GET: api/Language/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Language>> GetLanguage(int id)
        {
            var language = await _context.Languages.FindAsync(id);

            if (language == null)
            {
                return NotFound();
            }

            return language;
        }

        // POST: api/Language
        [HttpPost]
        public async Task<ActionResult<Language>> PostLanguage(Language language)
        {
            _context.Languages.Add(language);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLanguage), new { id = language.LanguageID }, language);
        }

        // PUT: api/Language/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLanguage(int id, [FromBody] Language updatedLanguage)
        {
            if (id != updatedLanguage.LanguageID)
            {
                return BadRequest();
            }

            var language = await _context.Languages.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            // Update the properties of the existing language entity with the values from the updatedLanguage object
            language.NameEn = updatedLanguage.NameEn;
            language.NameAr = updatedLanguage.NameAr;
            language.NameUr = updatedLanguage.NameUr;
            language.LanguageCode = updatedLanguage.LanguageCode;
            language.IsActive = updatedLanguage.IsActive;

            _context.Entry(language).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(id))
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

        // PATCH: api/Language/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchLanguage(int id, [FromBody] JsonPatchDocument<Language> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var languageToUpdate = await _context.Languages.FindAsync(id);
            if (languageToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the language entity
                patchDocument.ApplyTo(languageToUpdate,
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

        // DELETE: api/Language/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            var language = await _context.Languages.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LanguageExists(int id)
        {
            return _context.Languages.Any(e => e.LanguageID == id);
        }
    }
}
