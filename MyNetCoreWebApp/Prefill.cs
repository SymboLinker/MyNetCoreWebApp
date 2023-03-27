namespace MyNetCoreWebApp;

public static class Prefill
{
    public static async Task FunDatabase(WebApplication app)
    {
        var db = app.Services.GetService<IFunDb>() ?? throw new NullReferenceException(nameof(IFunDb));
        var programmingWorkshopId = await db.InsertAsync(new Workshop
        {
            Name = "Programming for beginners",
            Description = "Copy code examples and make small changes. Ýou'll learn how to use the NuGet package 'IGet', how to make a feature folder structure and write basic unit tests.",
            DateTimeStart = DateTime.Now.AddHours(1).AddMinutes(-DateTime.Now.Minute + 15),
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "Bill",
            WorkshopId = programmingWorkshopId,
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "Steve",
            WorkshopId = programmingWorkshopId,
        });
        var chessWorkshopId = await db.InsertAsync(new Workshop
        {
            Name = "Chess tactics",
            Description = "Improve your chess problem solving skills by recognizing patterns such as forks, pins and X-rays.",
            DateTimeStart = DateTime.Now.AddHours(4).AddMinutes(-DateTime.Now.Minute),
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "Bobby",
            WorkshopId = chessWorkshopId,
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "Gary",
            WorkshopId = chessWorkshopId,
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "Magnus",
            WorkshopId = chessWorkshopId,
        });
        var mathWorkshopId = await db.InsertAsync(new Workshop
        {
            Name = "Creativity in math",
            Description = "Learn how the application of heuristics in mathematics can stimulate creativity.",
            DateTimeStart = DateTime.Now.AddHours(1).AddMinutes(-DateTime.Now.Minute + 30),
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "Henry",
            WorkshopId = mathWorkshopId,
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "Andrew",
            WorkshopId = mathWorkshopId,
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "Terrence",
            WorkshopId = mathWorkshopId,
        });
        var gritWorkshopId = await db.InsertAsync(new Workshop
        {
            Name = "Creativity and grit",
            Description = "Get a concrete understanding of willpower and creative processes in order to reach your goals.",
            DateTimeStart = DateTime.Now.AddYears(-1).AddMinutes(-DateTime.Now.Minute),
        });
        await db.InsertAsync(new WorkshopParticipant
        {
            Name = "You",
            WorkshopId = gritWorkshopId,
        });
    }
}
