using Microsoft.AspNetCore.Mvc;
using VinModifierApp.Services.Augment;
using VinModifierApp.Services.VinService;

namespace VinModifierApp.WebApi.Controllers;

[ApiController]
[Route("api/vin")]
public class VinController : ControllerBase
{
    private readonly ILogger<VinController> Logger;
    public IVinService VinService { get; }
    public IAugmentService AugmentService { get; }

    public VinController(
        ILogger<VinController> logger,
        IVinService vinService,
        IAugmentService augmentService)
    {
        Logger = logger;
        VinService = vinService;
        AugmentService = augmentService;
    }


    /// <param name="files">Max amount to send is 3 files</param>
    [HttpPost("upload")]
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
    [HttpPut("augment")]
    public async Task<IActionResult> Augment()
    {
        try
        {
            var res = await AugmentService.AugmentAll();
            if (res)
                return Ok("The operation has completed successfully");

            return StatusCode(500, "The operation failed please check the logs");
        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return StatusCode(500, "Unhandled internal server error please check the logs");
        }

    }
}