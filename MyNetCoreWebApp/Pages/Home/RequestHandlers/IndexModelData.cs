namespace MyNetCoreWebApp.Pages.Welcome.RequestHandlers;

public class IndexModelDataRequestHandler
{
    private readonly IFunDb db;

    public IndexModelDataRequestHandler(IFunDb db)
    {
        this.db = db;
    }

    public async Task<IndexModelData> HandleAsync()
    {
        var now = DateTime.Now;
        var upcomingWorkshops = await db.QueryAsync<Workshop>(records => records
            .Where(x => x.DateTimeStart > now)
            .OrderBy(x => x.DateTimeStart)
            .Take(10));
        return new IndexModelData { UpcomingWorkshops = upcomingWorkshops.ToArray() };
    }
}

public class IndexModelData
{
    public Workshop[] UpcomingWorkshops { get; set; } = null!;
}
