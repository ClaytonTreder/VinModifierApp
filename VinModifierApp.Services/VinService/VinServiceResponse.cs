using System;

namespace VinModifierApp.Services.VinService;

public class VinServiceResponse
{
    public List<string> FailedFiles { get; set; } = new List<string>();
    public bool IsSuccess => FailedFiles.Count() == 0;
    public string ValidationMessage =>
        IsSuccess ?
        "Success" :
        $"The following fails failed validation {string.Join(",", FailedFiles)}";
}
