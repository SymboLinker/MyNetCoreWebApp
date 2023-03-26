namespace MyNetCoreWebApp.Pages.Welcome.RequestHandlers;

public class GetUpcomingWorkshops
{
    private readonly IFunDb db;

    public GetUpcomingWorkshops(IFunDb db)
    {
        this.db = db;
    }

    public async Task<Workshop[]> HandleAsync()
    {
        var now = DateTime.Now;
        var upcomingWorkshops = await db.QueryAsync<Workshop>(records => records
            .Where(x => x.DateTimeStart > now)
            .OrderBy(x => x.DateTimeStart)
            .Take(10));
        return upcomingWorkshops.ToArray();
    }
}
