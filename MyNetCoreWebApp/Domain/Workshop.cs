namespace MyNetCoreWebApp.Domain;

public class Workshop
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime DateTimeStart { get; set; }
}
