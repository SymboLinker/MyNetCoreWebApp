namespace MyNetCoreWebApp.Domain;

public class WorkshopParticipant
{
    public int Id { get; set; }
    public int WorkshopId { get; set; }
    public string Name { get; set; } = "";
}
