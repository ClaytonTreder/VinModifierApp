using Microsoft.AspNetCore.Mvc;
using VinModifierApp.Services.VinService;

namespace VinModifierApp.WebApi.Controllers;

[ApiController]
[Route("api/vin")]
public class VinController : ControllerBase
{
    private readonly ILogger<VinController> Logger;
    public IVinService VinService { get; }

    public VinController(ILogger<VinController> logger, IVinService vinService)
    {
        Logger = logger;
        VinService = vinService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile[] files)
    {
        try
        {
            if (files.Length > 3)
                return BadRequest("Max file count is 3");

            var res = await VinService.SaveVinInfo(files);
            if (res.IsSuccess)
                return Ok(res);

            return BadRequest(res);
        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return StatusCode(500, "Unhandled internal server error please check the logs");
        }

    }
}