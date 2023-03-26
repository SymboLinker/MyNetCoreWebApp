using MyNetCoreWebApp.Pages.Welcome.RequestHandlers;

namespace MyNetCoreWebApp.Tests.Pages.Home.RequestHandlers;

public class WorkshopListRequestHandlerTests : TestBase
{
    [Fact]
    public async void Gets_workshops_that_start_in_future()
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
        var workshops = await i.Get<GetUpcomingWorkshops>().HandleAsync();

        // Assert
        var workshop = Assert.Single(workshops);
        Assert.Equal("Programming for beginners", workshop.Name);
    }

    [Fact]
    public async void Takes_10()
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
        var workshops = await i.Get<GetUpcomingWorkshops>().HandleAsync();

        // Assert
        Assert.Equal(10, workshops.Length);
        foreach(var i in new int[] { 11, 12, 13, 14, 15 })
        {
            Assert.DoesNotContain(i.ToString(), workshops.Select(x => x.Name));
        }
    }
}