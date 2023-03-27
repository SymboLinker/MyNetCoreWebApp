namespace MyNetCoreWebApp.Pages.WorkshopDetails.RequestHandlers;

public record IndexModelDataRequest(int WorkshopId);

public class IndexModelDataRequestHandler
{
    private readonly IFunDb db;

    public IndexModelDataRequestHandler(IFunDb db)
    {
        this.db = db;
    }

    public async Task<Result<IndexModelData>> HandleAsync(IndexModelDataRequest request)
    {
        var workshop = await db.QuerySingleOrDefaultAsync<Workshop>(x => x.Id == request.WorkshopId);
        var participants = await db.QueryAsync<WorkshopParticipant>(records => records.Where(x => x.WorkshopId == request.WorkshopId));
        if (workshop is null)
        {
            return Result.Fail<IndexModelData>($"A workshop with Id '{request.WorkshopId}' could not be found.");
        }
        return Result.Success(new IndexModelData
        {
            Workshop = workshop,
            Participants = participants.ToArray(),
        });
    }
}

public class IndexModelData
{
    public Workshop Workshop { get; set; } = null!;
    public WorkshopParticipant[] Participants { get; set; } = null!;
}
