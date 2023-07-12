using Microsoft.AspNetCore.Mvc;
using VinModifierApp.Models;

namespace VinModifierApp.WebApi.Controllers;

[ApiController]
[Route("api/vehicle")]
public class VehicleController : ControllerBase
{
    private readonly ILogger<VehicleController> Logger;

    public VehicleController(ILogger<VehicleController> logger)
    {
        Logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IEnumerable<VehicleModel>> Get(int id)
    {

        return new List<VehicleModel>();
    }
}