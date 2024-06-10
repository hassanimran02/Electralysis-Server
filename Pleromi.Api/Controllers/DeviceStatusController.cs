
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
    public class DeviceStatusController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public DeviceStatusController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/DeviceStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceStatus>>> GetDeviceStatuses()
        {
            return await _context.DeviceStatuses.ToListAsync();
        }

        // GET: api/DeviceStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceStatus>> GetDeviceStatus(int id)
        {
            var deviceStatus = await _context.DeviceStatuses.FindAsync(id);

            if (deviceStatus == null)
            {
                return NotFound();
            }

            return deviceStatus;
        }

        // POST: api/DeviceStatus
        [HttpPost]
        public async Task<ActionResult<DeviceStatus>> PostDeviceStatus(DeviceStatus deviceStatus)
        {
            _context.DeviceStatuses.Add(deviceStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDeviceStatus), new { id = deviceStatus.DeviceStatusID }, deviceStatus);
        }

        // PUT: api/DeviceStatus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceStatus(int id, [FromBody] DeviceStatus updatedDeviceStatus)
        {
            if (id != updatedDeviceStatus.DeviceStatusID)
            {
                return BadRequest();
            }

            var deviceStatus = await _context.DeviceStatuses.FindAsync(id);
            if (deviceStatus == null)
            {
                return NotFound();
            }

            // Update the properties of the existing deviceStatus entity with the values from the updatedDeviceStatus object
            deviceStatus.DeviceID = updatedDeviceStatus.DeviceID;
            deviceStatus.StatusID = updatedDeviceStatus.StatusID;
            deviceStatus.LastUpdatedOn = updatedDeviceStatus.LastUpdatedOn;
            deviceStatus.Reason = updatedDeviceStatus.Reason;

            _context.Entry(deviceStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceStatusExists(id))
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

        // PATCH: api/DeviceStatus/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDeviceStatus(int id, [FromBody] JsonPatchDocument<DeviceStatus> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var deviceStatusToUpdate = await _context.DeviceStatuses.FindAsync(id);
            if (deviceStatusToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the deviceStatus entity
                patchDocument.ApplyTo(deviceStatusToUpdate,
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

        // DELETE: api/DeviceStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceStatus(int id)
        {
            var deviceStatus = await _context.DeviceStatuses.FindAsync(id);
            if (deviceStatus == null)
            {
                return NotFound();
            }

            _context.DeviceStatuses.Remove(deviceStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceStatusExists(int id)
        {
            return _context.DeviceStatuses.Any(e => e.DeviceStatusID == id);
        }
    }
}
