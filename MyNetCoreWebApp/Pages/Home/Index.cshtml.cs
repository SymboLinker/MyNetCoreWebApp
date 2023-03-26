using Microsoft.AspNetCore.Mvc.RazorPages;
using MyNetCoreWebApp.Pages.Welcome.RequestHandlers;

namespace MyNetCoreWebApp.Pages;
public class IndexModel : PageModel
{
    private readonly IGet i;

    public IndexModel(IGet iget)
    {
        i = iget;
    }

    public Workshop[] Workshops { get; set; } = null!;

    public async Task OnGet()
    {
        var request = new WorkshopListRequest { DateTimeStartFrom = DateTime.Now };
        Workshops = await i.Get<WorkshopListRequestHandler>().HandleAsync(request);
    }
}
