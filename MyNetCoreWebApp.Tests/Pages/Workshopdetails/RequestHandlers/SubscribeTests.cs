using MyNetCoreWebApp.Pages.WorkshopDetails.RequestHandlers;

namespace MyNetCoreWebApp.Tests.Pages.Workshopdetails.RequestHandlers;
public class SubscribeTests : TestBase
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var workshopId = await db.InsertAsync(new Workshop
        {
            Name = "Programming for beginners",
            DateTimeStart = DateTime.Now.AddHours(1),
        });

        // Act
        var request = new SubscribeRequest("Bill", workshopId);
        var result = await i.Get<SubscribeRequestHandler>().HandleAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        var participants = await db.QueryAsync<WorkshopParticipant>(records => records.Where(x => x.WorkshopId == workshopId));
        var participant = Assert.Single(participants);
        Assert.Equal("Bill", participant.Name);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("A", false)]
    [InlineData("A ", false)]
    [InlineData(" A", false)]
    [InlineData("Ab", true)]
    public async Task Validates_name_input_length(string nameInput, bool shouldSucceed)
    {
        // Arrange
        var workshopId = await db.InsertAsync(new Workshop
        {
            Name = "Programming for beginners",
            DateTimeStart = DateTime.Now.AddHours(1),
        });

        // Act
        var request = new SubscribeRequest(nameInput, workshopId);
        var result = await i.Get<SubscribeRequestHandler>().HandleAsync(request);

        // Assert
        if (shouldSucceed)
        {
            Assert.True(result.IsSuccess);
        }
        else
        {
            Assert.True(result.IsFail);
            Assert.Equal("You should specify a name of at least 2 characters.", result.ErrorMessage);
        }
    }

    [Fact]
    public async Task Disallows_participants_to_have_the_same_name()
    {
        // Arrange
        var workshopId = await db.InsertAsync(new Workshop
        {
            Name = "Programming for beginners",
            DateTimeStart = DateTime.Now.AddHours(1),
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "Bill",
            WorkshopId = workshopId,
        });

        // Act
        var request = new SubscribeRequest("Bill", workshopId);
        var result = await i.Get<SubscribeRequestHandler>().HandleAsync(request);

        // Assert
        Assert.True(result.IsFail);
        Assert.Equal("A participant with the name 'Bill' is in the workshop already. Strict rules disallow participants to use the same name.", result.ErrorMessage);
    }
}
