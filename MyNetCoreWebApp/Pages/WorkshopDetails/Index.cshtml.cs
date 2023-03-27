using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyNetCoreWebApp.Pages.WorkshopDetails.RequestHandlers;

namespace MyNetCoreWebApp.Pages.WorkshopDetails;
public class IndexModel : PageModel
{
    private readonly IGet i;

    public IndexModel(IGet iget)
    {
        i = iget;
    }

    public IndexModelData Data { get; set; } = null!;

    public async Task<IActionResult> OnGet([FromRoute] IndexModelDataRequest request)
    {
        var result = await i.Get<IndexModelDataRequestHandler>().HandleAsync(request);
        if (result.IsFail)
        {
            TempData[Constants.ErrorMessage] = result.ErrorMessage;
            return Redirect("/");
        }
        else
        {
            Data = result.Value;
            return Page();
        }
    }

    public async Task<IActionResult> OnPost(SubscribeRequest request)
    {
        var result = await i.Get<SubscribeRequestHandler>().HandleAsync(request);
        if (result.IsFail)
        {
            TempData[Constants.ErrorMessage] = result.ErrorMessage;
        }
        else
        {
            TempData[Constants.SuccessMessage] = "You have been succesfully subscribed to this workshop.";
        }
        return Redirect($"/workshops/details/{request.WorkshopId}");
    }
}
