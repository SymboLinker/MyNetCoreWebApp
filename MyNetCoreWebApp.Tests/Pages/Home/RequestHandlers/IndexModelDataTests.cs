using MyNetCoreWebApp.Pages.Welcome.RequestHandlers;

namespace MyNetCoreWebApp.Tests.Pages.Home.RequestHandlers;

public class IndexModelDataTests : TestBase
{
    [Fact]
    public async Task Gets_workshops_that_start_in_future()
    {
        // Arrange
        await db.InsertAsync(new Workshop
        {
            Name = "Chess tactics",
            DateTimeStart = DateTime.Now.AddHours(-1),
        });
        await db.InsertAsync(new Workshop
        {
            Name = "Programming for beginners",
            DateTimeStart = DateTime.Now.AddHours(1),
        });

        // Act
        var data = await i.Get<IndexModelDataRequestHandler>().HandleAsync();
        var workshops = data.UpcomingWorkshops;

        // Assert
        var workshop = Assert.Single(workshops);
        Assert.Equal("Programming for beginners", workshop.Name);
    }

    [Fact]
    public async Task Takes_10()
    {
        // Arrange
        for(var i = 1; i < 15; i++)
        {
            await db.InsertAsync(new Workshop
            {
                Name = "Programming level " + i,
                DateTimeStart = DateTime.Now.AddHours(i),
            });
        }

        // Act
        var data = await i.Get<IndexModelDataRequestHandler>().HandleAsync();
        var workshops = data.UpcomingWorkshops;

        // Assert
        Assert.Equal(10, workshops.Length);
        foreach(var i in new int[] { 11, 12, 13, 14, 15 })
        {
            Assert.DoesNotContain(i.ToString(), workshops.Select(x => x.Name));
        }
    }
}