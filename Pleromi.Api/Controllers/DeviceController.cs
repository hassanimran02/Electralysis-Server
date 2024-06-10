
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
    public class DeviceController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public DeviceController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // GET: api/Device
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        // GET: api/Device/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        // POST: api/Device
        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDevice), new { id = device.DeviceID }, device);
        }

        // PUT: api/Device/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(int id, [FromBody] Device updatedDevice)
        {
            if (id != updatedDevice.DeviceID)
            {
                return BadRequest();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            // Update the properties of the existing device entity with the values from the updatedDevice object
            device.ConsumerTypeID = updatedDevice.ConsumerTypeID;
            device.DeviceTypeID = updatedDevice.DeviceTypeID;
            device.DeviceName = updatedDevice.DeviceName;
            device.UserID = updatedDevice.UserID;
            device.MacAddress = updatedDevice.MacAddress;
            device.ModifiedOn = DateTime.UtcNow; // Assuming modification timestamp is updated on PUT
            device.ModifiedBy = updatedDevice.ModifiedBy;

            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

        // PATCH: api/Device/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDevice(int id, [FromBody] JsonPatchDocument<Device> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var deviceToUpdate = await _context.Devices.FindAsync(id);
            if (deviceToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the device entity
                patchDocument.ApplyTo(deviceToUpdate,
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

        // DELETE: api/Device/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            // Soft delete by setting the IsDeleted flag to true
            device.IsDeleted = true;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.DeviceID == id);
        }
    }
}
