using System;
using VinModifierApp.Models;

namespace VinModifierApp.NHTSAApi;
public class NHTSAModel
{
    public int Count { get; set; }
    public string Message { get; set; } = string.Empty;
    public string SearchCriteria { get; set; } = string.Empty;
    public IEnumerable<VehicleModel> Results { get; set; } = Enumerable.Empty<VehicleModel>();
}
