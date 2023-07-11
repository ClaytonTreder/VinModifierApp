using Microsoft.AspNetCore.Mvc;

namespace VinModifierApp.WebApi.Controllers;
[ApiController]
[Route("api/vehicle")]
public class VehicleController : ControllerBase
{

    private readonly ILogger<VehicleController> _logger;

    public VehicleController(ILogger<VehicleController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<object> Get()
    {
        return new List<object>();
    }
}