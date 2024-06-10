
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
    public class DeviceTypeController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public DeviceTypeController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/DeviceType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceType>>> GetDeviceTypes()
        {
            return await _context.DeviceTypes.ToListAsync();
        }

        // GET: api/DeviceType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceType>> GetDeviceType(int id)
        {
            var deviceType = await _context.DeviceTypes.FindAsync(id);

            if (deviceType == null)
            {
                return NotFound();
            }

            return deviceType;
        }

        // POST: api/DeviceType
        [HttpPost]
        public async Task<ActionResult<DeviceType>> PostDeviceType(DeviceType deviceType)
        {
            _context.DeviceTypes.Add(deviceType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDeviceType), new { id = deviceType.DeviceTypeID }, deviceType);
        }

        // PUT: api/DeviceType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceType(int id, [FromBody] DeviceType updatedDeviceType)
        {
            if (id != updatedDeviceType.DeviceTypeID)
            {
                return BadRequest();
            }

            var deviceType = await _context.DeviceTypes.FindAsync(id);
            if (deviceType == null)
            {
                return NotFound();
            }

            // Update the properties of the existing device type entity with the values from the updatedDeviceType object
            deviceType.NameEn = updatedDeviceType.NameEn;
            deviceType.NameAr = updatedDeviceType.NameAr;
            deviceType.NameUr = updatedDeviceType.NameUr;
            deviceType.IsActive = updatedDeviceType.IsActive;


            _context.Entry(deviceType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceTypeExists(id))
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

        // PATCH: api/DeviceType/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDeviceType(int id, [FromBody] JsonPatchDocument<DeviceType> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var deviceTypeToUpdate = await _context.DeviceTypes.FindAsync(id);
            if (deviceTypeToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the device type entity
                patchDocument.ApplyTo(deviceTypeToUpdate,
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

        // DELETE: api/DeviceType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceType(int id)
        {
            var deviceType = await _context.DeviceTypes.FindAsync(id);
            if (deviceType == null)
            {
                return NotFound();
            }

            _context.DeviceTypes.Remove(deviceType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceTypeExists(int id)
        {
            return _context.DeviceTypes.Any(e => e.DeviceTypeID == id);
        }
    }
}
