using System;
using Newtonsoft.Json;
using VinModifierApp.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VinModifierApp.NHTSAApi;

public class NHTSA
{
    private readonly HttpClient HttpClient;

    public ILogger<NHTSA> Logger { get; }

    public NHTSA(ILogger<NHTSA> logger)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://vpic.nhtsa.dot.gov");
        HttpClient = httpClient;
        Logger = logger;
    }


    public async Task<VehicleModel?> GetVehicle(string vin)
    {
        try
        {
            var res = await HttpClient.GetAsync($"/api/vehicles/DecodeVinValues/{vin}?format=json");
            res.EnsureSuccessStatusCode();

            var vehicle = JsonConvert.DeserializeObject<NHTSAModel>(
                await res.Content.ReadAsStringAsync())?.Results.FirstOrDefault();

            return vehicle;
        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<VehicleModel>?> GetVehicles(string[] vins)
    {
        try
        {
            var data = new Dictionary<string, string>()
            {
                { "DATA", string.Join(";", vins) },
                { "format", "JSON" }
            };
            var content = new FormUrlEncodedContent(data);
            var res = await HttpClient.PostAsync(
                $"/api/vehicles/DecodeVINValuesBatch/", content);

            res.EnsureSuccessStatusCode();

            var vehicles = JsonConvert.DeserializeObject<NHTSAModel>(
                await res.Content.ReadAsStringAsync())?.Results;

            return vehicles;
        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return null;
        }
    }
    private class GetVehiclesBody
    {
        public string DATA { get; set; } = string.Empty;
        public string format { get; set; } = "JSON";
    }
}
