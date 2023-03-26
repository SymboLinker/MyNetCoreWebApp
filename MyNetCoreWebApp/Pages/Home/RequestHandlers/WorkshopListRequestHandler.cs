using FunDb;

namespace MyNetCoreWebApp.Pages.Welcome.RequestHandlers;

public class WorkshopListRequest
{
    public DateTime DateTimeStartFrom { get; set; }
}
public class WorkshopListRequestHandler
{
    private readonly IFunDb db;

    public WorkshopListRequestHandler(IFunDb db)
    {
        this.db = db;
    }

    public async Task<Workshop[]> HandleAsync(WorkshopListRequest request)
    {
        var workshops = await db.QueryAsync<Workshop>(x => x.DateTimeStart > request.DateTimeStartFrom);
        return workshops.OrderBy(x => x.DateTimeStart).Take(10).ToArray();
    }
}
