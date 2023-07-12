using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VinModifierApp.FileStorage;

namespace VinModifierApp.FileStorage.Disk
{
    public class DiskFileStorage : IFileStorage
    {
        public ILogger<DiskFileStorage> Logger { get; }

        public DiskFileStorage(ILogger<DiskFileStorage> logger)
        {
            Logger = logger;
        }

        public async Task<bool> SaveFile(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var fileStream = new FileStream($"Archive/Archive_{DateTime.UtcNow:MM_dd_yyyy_HH_mm}_{file.FileName}", FileMode.CreateNew))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(new EventId(), ex, ex.Message);
                return false;
            }
        }
    }
}

