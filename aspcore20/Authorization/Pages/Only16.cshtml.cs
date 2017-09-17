using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.Pages
{
    [Authorize("Is16")]
    public class Only16Model : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Heute Abend, auf der Party!";
        }
    }
}
