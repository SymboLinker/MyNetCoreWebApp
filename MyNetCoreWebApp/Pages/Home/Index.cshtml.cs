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

    public Workshop[] UpcomingWorkshops { get; set; } = null!;

    public async Task OnGet()
    {
        UpcomingWorkshops = await i.Get<GetUpcomingWorkshops>().HandleAsync();
    }
}
