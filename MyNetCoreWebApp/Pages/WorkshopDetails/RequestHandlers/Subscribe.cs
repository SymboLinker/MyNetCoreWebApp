namespace MyNetCoreWebApp.Pages.WorkshopDetails.RequestHandlers;

public record SubscribeRequest(string Name, int WorkshopId);

public class SubscribeRequestHandler
{
    private readonly IFunDb db;
    private readonly IGet i;

    public SubscribeRequestHandler(IFunDb db, IGet iget)
    {
        this.db = db;
        i = iget;
    }

    public async Task<Result> HandleAsync(SubscribeRequest request)
    {
        var validationResult = await i.Get<SubscribeRequestValidator>().ValidateAsync(request);
        if (validationResult.IsFail)
        {
            return validationResult;
        }

        await db.InsertAsync(new WorkshopParticipant
        {
            Name = request.Name.Trim(),
            WorkshopId = request.WorkshopId,
        });

        return Result.Success();
    }
}

public class SubscribeRequestValidator
{
    private readonly IFunDb db;

    public SubscribeRequestValidator(IFunDb db)
    {
        this.db = db;
    }

    const int MinimumNameLength = 2;
    const int MaxNumberOfParticipants = 5;
    public async Task<Result> ValidateAsync(SubscribeRequest request)
    {
        var workshop = await db.QuerySingleOrDefaultAsync<Workshop>(x => x.Id == request.WorkshopId);
        if (workshop is null)
        {
            return Result.Fail($"Something went wrong. A workhop with Id '{request.WorkshopId}' does not exist.");
        }

        var name = request.Name?.Trim();

        var validationResult = new Result();

        if (string.IsNullOrEmpty(name) || name.Length < MinimumNameLength)
        {
            validationResult.ErrorMessages.Add($"You should specify a name of at least {MinimumNameLength} characters.");
        }

        var participants = await db.QueryAsync<WorkshopParticipant>(records => records.Where(x => x.WorkshopId == request.WorkshopId));
        if (participants.Count() >= MaxNumberOfParticipants)
        {
            validationResult.ErrorMessages.Add($"This workshop is full. The maximum number of participants is {MaxNumberOfParticipants}.");
        }

        if (participants.Any(x => x.Name == name))
        {
            validationResult.ErrorMessages.Add($"A participant with the name '{name}' is in the workshop already. Strict rules disallow participants to use the same name.");
        }

        if (validationResult.IsFail)
        {
            return validationResult;
        }

        return Result.Success();
    }
}