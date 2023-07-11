using Microsoft.Extensions.Logging.Abstractions;
using VinModifierApp.Models;
using VinModifierApp.Services.Import;

namespace VinModifierApp.Services.Test.ImportServiceTests;

public class ImportServiceTest
{

    [Fact]
    public void CSV_Does_Have_Values()
    {
        // arrage
        var importService = new Mock<ImportService>(new NullLogger<ImportService>());

        // act

        var vehicles = importService.Object.GetImportVehicles();

        // assert

        Assert.NotNull(vehicles);
        Assert.NotEmpty(vehicles);
    }
}