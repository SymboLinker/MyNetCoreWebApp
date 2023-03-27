using MyNetCoreWebApp.Pages.WorkshopDetails.RequestHandlers;

namespace MyNetCoreWebApp.Tests.Pages.Workshopdetails.RequestHandlers;

public class IndexModelDataTests : TestBase
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var workshopId = await db.InsertAsync(new Workshop
        {
            Name = "Chess tactics",
            DateTimeStart = DateTime.Now.AddHours(-1),
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "Bobby",
            WorkshopId = workshopId,
        });

        // Act
        var request = new IndexModelDataRequest(workshopId);
        var result = await i.Get<IndexModelDataRequestHandler>().HandleAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var data = result.Value;
        Assert.NotNull(data.Workshop);
        var workshop = data.Workshop;
        Assert.Equal("Chess tactics", workshop.Name);
        var particpant = Assert.Single(data.Participants);
        Assert.Equal("Bobby", particpant.Name);
    }

    [Fact]
    public async Task Returns_an_error_message_if_the_workshop_does_not_exist()
    {
        // Act
        var request = new IndexModelDataRequest(WorkshopId: 31415);
        var result = await i.Get<IndexModelDataRequestHandler>().HandleAsync(request);

        // Assert
        Assert.True(result.IsFail);
        Assert.Equal("A workshop with Id '31415' could not be found.", result.ErrorMessage);
    }
}