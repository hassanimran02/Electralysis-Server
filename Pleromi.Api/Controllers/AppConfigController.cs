using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Pleromi.DAL.Entities;
using Pleromi.DAL.DatabaseContext;

namespace Pleromi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppConfigController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public AppConfigController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/AppConfig
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppConfig>>> GetAppConfigs()
        {
            return await _context.AppConfigs.ToListAsync();
        }

        // GET: api/AppConfig/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppConfig>> GetAppConfig(int id)
        {
            var appConfig = await _context.AppConfigs.FindAsync(id);

            if (appConfig == null)
            {
                return NotFound("AppConfig not found");
            }

            return appConfig;
        }

        // POST: api/AppConfig
        [HttpPost]
        public async Task<ActionResult<AppConfig>> PostAppConfig(AppConfig appConfig)
        {
            _context.AppConfigs.Add(appConfig);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppConfig), new { id = appConfig.AppConfigID }, appConfig);
        }

        // PUT: api/AppConfig/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppConfig(int id, AppConfig updatedAppConfig)
        {
            if (id != updatedAppConfig.AppConfigID)
            {
                return BadRequest();
            }

            var appConfig = await _context.AppConfigs.FindAsync(id);
            if (appConfig == null)
            {
                return NotFound();
            }

            // Update the properties of the existing appConfig entity with the values from the updatedAppConfig object
            appConfig.AppConfigKey = updatedAppConfig.AppConfigKey;
            appConfig.AppConfigValueEn = updatedAppConfig.AppConfigValueEn;
            appConfig.AppConfigValueAr = updatedAppConfig.AppConfigValueAr;
            appConfig.AppConfigValueUr = updatedAppConfig.AppConfigValueUr;
            appConfig.IsActive = updatedAppConfig.IsActive;

            _context.Entry(appConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppConfigExists(id))
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

        // PATCH: api/AppConfig/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAppConfig(int id, [FromBody] JsonPatchDocument<AppConfig> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var appConfigToUpdate = await _context.AppConfigs.FindAsync(id);
            if (appConfigToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the appConfig entity
                patchDocument.ApplyTo(appConfigToUpdate,
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

        // DELETE: api/AppConfig/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppConfig(int id)
        {
            var appConfig = await _context.AppConfigs.FindAsync(id);
            if (appConfig == null)
            {
                return NotFound();
            }

            _context.AppConfigs.Remove(appConfig);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppConfigExists(int id)
        {
            return _context.AppConfigs.Any(e => e.AppConfigID == id);
        }
    }
}
