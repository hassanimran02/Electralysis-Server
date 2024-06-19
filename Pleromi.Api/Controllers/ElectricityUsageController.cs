
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
    public class ElectricityUsageController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public ElectricityUsageController(ElectralysisDbContext context)
        {
            _context = context;
        }

        private const double OnPeakRate = 41.8900;
        private const double OffPeakRate = 35.5700;

        public static double? CalculateCostForDay(DateTime day, List<ElectricityUsage> usages)
        {
            double totalCost = 0;

            foreach (var usage in usages)
            {
                totalCost += CalculateCost(usage.UsageOn, usage.Unit) ?? 0;
            }

            return totalCost;
        }

        private double? CalculateCostForWeek(int year, DateTime weekStart, List<ElectricityUsage> usages)
        {
            double totalCost = 0;

            foreach (var usage in usages)
            {
                totalCost += CalculateCost(usage.UsageOn, usage.Unit) ?? 0;
            }

            return totalCost;
        }

        private double? CalculateCostForMonth(int year, int month, List<ElectricityUsage> usages)
        {
            double totalCost = 0;

            foreach (var usage in usages)
            {
                totalCost += CalculateCost(usage.UsageOn, usage.Unit) ?? 0;
            }

            return totalCost;
        }


        public static double? CalculateCost(DateTime usageTime, double? units)
        {
            double? rate;

            if (usageTime.Month >= 4 && usageTime.Month <= 10) // April to October
            {
                if (usageTime.TimeOfDay >= new TimeSpan(18, 30, 0) && usageTime.TimeOfDay <= new TimeSpan(22, 30, 0))
                {
                    rate = OnPeakRate;
                }
                else
                {
                    rate = OffPeakRate;
                }
            }
            else // November to March
            {
                if (usageTime.TimeOfDay >= new TimeSpan(18, 0, 0) && usageTime.TimeOfDay <= new TimeSpan(22, 0, 0))
                {
                    rate = OnPeakRate;
                }
                else
                {
                    rate = OffPeakRate;
                }
            }

            return units * rate;
        }

        // GET: api/ElectricityUsage/LatestUnit
        [HttpGet("LatestUnit")]
        public async Task<ActionResult<double?>> GetLatestUnit()
        {
            var latestUsage = await _context.ElectricityUsages
                .OrderByDescending(e => e.UsageOn)
                .FirstOrDefaultAsync();

            if (latestUsage == null)
            {
                return NotFound();
            }

            return latestUsage.Unit;
        }

        // GET: api/ElectricityUsage/Daily?date={date}
        [HttpGet("Daily")]
        public async Task<ActionResult<IEnumerable<DailyData>>> GetDailyData(DateTime date)
        {
            try
            {
                // Calculate the start date (7 days ago from the input date)
                var startDate = date.Date.AddDays(-6);

                // Fetch data from the database for the past 7 days (including the input date)
                var dailyData = await _context.ElectricityUsages
                    .Where(e => e.UsageOn >= startDate && e.UsageOn <= date.AddDays(1)) // Ensure we cover the entire date range
                    .GroupBy(e => new { e.UsageOn.Year, e.UsageOn.Month, e.UsageOn.Day })
                    .Select(g => new DailyData
                    {
                        Day = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                        DayOfWeek = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day).DayOfWeek.ToString(),
                        UnitsUsed = g.Sum(e => e.Unit),
                        Cost = CalculateCostForDay(new DateTime(g.Key.Year, g.Key.Month, g.Key.Day), g.ToList())
                    })
                    .ToListAsync();

                // Reorder the list to ensure the current day's data appears first
                var currentDateData = dailyData.FirstOrDefault(data => data.Day.Date == date.Date);
                if (currentDateData != null)
                {
                    dailyData.Remove(currentDateData);
                    dailyData.Insert(0, currentDateData);
                }

                // Fill in missing days with zero usage
                for (int i = 0; i < 7; i++)
                {
                    var currentDate = startDate.AddDays(i);
                    if (!dailyData.Any(d => d.Day == currentDate))
                    {
                        dailyData.Add(new DailyData
                        {
                            Day = currentDate,
                            DayOfWeek = currentDate.DayOfWeek.ToString(),
                            UnitsUsed = 0,
                            Cost = 0
                        });
                    }
                }

                // Sort the data by date
                dailyData = dailyData.OrderByDescending(d => d.Day).ToList();

                // Return the data
                return Ok(dailyData);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching daily data: {ex}");

                // Return an error response
                return StatusCode(500, $"Error fetching daily data: {ex.Message}");
            }
        }

        // GET: api/ElectricityUsage/Weekly
        [HttpGet("Weekly")]
        public async Task<ActionResult<IEnumerable<WeeklyData>>> GetWeeklyData()
        {
            var today = DateTime.Today;
            var currentWeekStart = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday); // Start of current week
            var fourWeeksAgo = currentWeekStart.AddDays(-28); // Start of the week 3 weeks ago

            var weeklyData = await _context.ElectricityUsages
                .Where(e => e.UsageOn >= fourWeeksAgo && e.UsageOn < currentWeekStart.AddDays(7)) // 4 weeks data
                .ToListAsync();

            var groupedWeeklyData = weeklyData
                .GroupBy(e => new
                {
                    Year = e.UsageOn.Year,
                    WeekStart = e.UsageOn.AddDays(-(int)e.UsageOn.DayOfWeek + (int)DayOfWeek.Monday).Date
                })
                .Select(g => new WeeklyData
                {
                    WeekStart = g.Key.WeekStart,
                    Year = g.Key.Year,
                    UnitsUsed = g.Sum(e => e.Unit),
                    Cost = CalculateCostForWeek(g.Key.Year, g.Key.WeekStart, g.ToList())
                })
                .OrderByDescending(g => g.WeekStart) // Latest week first
                .ToList();

            // If no data is found for the current week, add a zero entry
            if (!groupedWeeklyData.Any(g => g.WeekStart == currentWeekStart))
            {
                groupedWeeklyData.Add(new WeeklyData
                {
                    WeekStart = currentWeekStart,
                    Year = currentWeekStart.Year,
                    UnitsUsed = 0,
                    Cost = 0
                });
            }


            return groupedWeeklyData;
        }

        // GET: api/ElectricityUsage/Monthly
        [HttpGet("Monthly")]
        public async Task<ActionResult<IEnumerable<MonthlyData>>> GetMonthlyData()
        {
            try
            {
                // Get the current date
                var currentDate = DateTime.Today;

                // Calculate the start date for the last 12 months
                var startDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-11);
                var endDate = currentDate.AddDays(1); // End date is exclusive

                // Fetch data from the database
                var usageData = await _context.ElectricityUsages
                    .Where(e => e.UsageOn >= startDate && e.UsageOn < endDate)
                    .ToListAsync();

                // Group and aggregate data in memory
                var monthlyData = usageData
                    .GroupBy(e => new { e.UsageOn.Year, e.UsageOn.Month })
                    .Select(g => new MonthlyData
                    {
                        Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                        UnitsUsed = g.Sum(e => e.Unit),
                        Cost = CalculateCostForMonth(g.Key.Year, g.Key.Month, g.ToList())
                    })
                    .OrderByDescending(m => m.Month)
                    .ToList();

                if (!monthlyData.Any(m => m.Month.Year == currentDate.Year && m.Month.Month == currentDate.Month))
                {
                    monthlyData.Add(new MonthlyData
                    {
                        Month = currentDate,
                        UnitsUsed = 0,
                        Cost = 0
                    });
                }

                return Ok(monthlyData);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching monthly data: {ex}");

                // Return an error response
                return StatusCode(500, $"Error fetching monthly data: {ex.Message}");
            }
        }

        // GET: api/ElectricityUsage/UnitsByDateTimeRange
        [HttpGet("UnitsByDateTimeRange")]
        public async Task<ActionResult<double>> GetUnitsByDateTimeRange(DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                // Fetch the sum of units from the database based on the specified datetime range
                var totalUnits = await _context.ElectricityUsages
                    .Where(e => e.UsageOn >= startDateTime && e.UsageOn <= endDateTime)
                    .SumAsync(e => e.Unit);

                return Ok(totalUnits);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching units by datetime range: {ex}");

                // Return an error response
                return StatusCode(500, $"Error fetching units by datetime range: {ex.Message}");
            }
        }



        // GET: api/ElectricityUsage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ElectricityUsage>>> GetElectricityUsages()
        {
            return await _context.ElectricityUsages.ToListAsync();
        }

        // GET: api/ElectricityUsage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ElectricityUsage>> GetElectricityUsage(int id)
        {
            var electricityUsage = await _context.ElectricityUsages.FindAsync(id);

            if (electricityUsage == null)
            {
                return NotFound();
            }

            return electricityUsage;
        }

        // POST: api/ElectricityUsage
        [HttpPost]
        public async Task<ActionResult<ElectricityUsage>> PostElectricityUsage(ElectricityUsage electricityUsage)
        {
            _context.ElectricityUsages.Add(electricityUsage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetElectricityUsage), new { id = electricityUsage.ElectricityUsageID }, electricityUsage);
        }

        // PUT: api/ElectricityUsage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElectricityUsage(int id, [FromBody] ElectricityUsage updatedElectricityUsage)
        {
            if (id != updatedElectricityUsage.ElectricityUsageID)
            {
                return BadRequest();
            }

            var electricityUsage = await _context.ElectricityUsages.FindAsync(id);
            if (electricityUsage == null)
            {
                return NotFound();
            }

            // Update the properties of the existing electricityUsage entity with the values from the updatedElectricityUsage object
            electricityUsage.DeviceID = updatedElectricityUsage.DeviceID;
            electricityUsage.Voltage = updatedElectricityUsage.Voltage;
            electricityUsage.Ampere = updatedElectricityUsage.Ampere;
            electricityUsage.Power = updatedElectricityUsage.Power;
            electricityUsage.UsageOn = updatedElectricityUsage.UsageOn;

            _context.Entry(electricityUsage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElectricityUsageExists(id))
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

        // PATCH: api/ElectricityUsage/6
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchElectricityUsage(int id, [FromBody] JsonPatchDocument<ElectricityUsage> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var electricityUsageToUpdate = await _context.ElectricityUsages.FindAsync(id);
            if (electricityUsageToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Apply the patch operations to the electricityUsage entity
                patchDocument.ApplyTo(electricityUsageToUpdate,
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

        // DELETE: api/ElectricityUsage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElectricityUsage(int id)
        {
            var electricityUsage = await _context.ElectricityUsages.FindAsync(id);
            if (electricityUsage == null)
            {
                return NotFound();
            }

            _context.ElectricityUsages.Remove(electricityUsage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ElectricityUsageExists(int id)
        {
            return _context.ElectricityUsages.Any(e => e.ElectricityUsageID == id);
        }
    }
}
