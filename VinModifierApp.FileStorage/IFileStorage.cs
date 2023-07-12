using System;
using Microsoft.AspNetCore.Http;

namespace VinModifierApp.FileStorage;

public interface IFileStorage
{
    public Task<bool> SaveFile(IFormFile file);
}
