using VinModifierApp.Data.Interfaces;
using VinModifierApp.Data.MongoDB;
using VinModifierApp.Data.MongoDB.Collections;
using VinModifierApp.FileStorage;
using VinModifierApp.FileStorage.Disk;
using VinModifierApp.FileStorage.S3;
using VinModifierApp.NHTSAApi;
using VinModifierApp.Services.Augment;
using VinModifierApp.Services.Data;
using VinModifierApp.Services.VinService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


ConfigureServices(builder.Services, builder.Configuration);


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddSingleton<IConfiguration>(configuration);

    services.AddScoped<IConnect, Connect>();
    services.AddScoped<IFileStorage, S3FileStorage>();
    services.AddScoped<IAugmentService, AugmentService>();
    services.AddScoped<IVehicleRepository, VehicleCollection>();
    services.AddScoped<IVinService, VinService>();
    services.AddScoped<IDataService, DataService>();
    services.AddScoped<NHTSA>();
}