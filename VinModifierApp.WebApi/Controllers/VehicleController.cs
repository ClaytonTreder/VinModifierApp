using Microsoft.AspNetCore.Mvc;
using VinModifierApp.Models;
using VinModifierApp.Services.Data;

namespace VinModifierApp.WebApi.Controllers;

[ApiController]
[Route("api/vehicle")]
public class VehicleController : ControllerBase
{
    private readonly ILogger<VehicleController> Logger;
    private readonly IDataService DataService;

    public VehicleController(ILogger<VehicleController> logger, IDataService dataService)
    {
        Logger = logger;
        DataService = dataService;
    }

    [HttpGet("{vin}")]
    public async Task<IActionResult> Get(string vin)
    {
        try
        {
            var vehicle = await DataService.GetByVin(vin);
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return StatusCode(500, "Unhandled internal server error please check the logs");

        }
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int start = 0,
        [FromQuery] int limit = 25,
        [FromQuery] int? dealerId = null,
        [FromQuery] DateTime? afterModifiedDate = null)
    {
        try
        {
            if (limit > 50)
                return BadRequest("Max return results is 50, please update query");

            var vehicles = await DataService.GetAll(start, limit, dealerId,
                afterModifiedDate == null ? null : DateOnly.FromDateTime(afterModifiedDate.Value));
            return Ok(vehicles);

        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return StatusCode(500, "Unhandled internal server error please check the logs");

        }
    }
}