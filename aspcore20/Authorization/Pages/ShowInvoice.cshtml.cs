using System.Threading.Tasks;
using Authorization.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.Pages
{
    public class ShowInvoiceModel : PageModel
    {
        private readonly IAuthorizationService _authorization;

        public ShowInvoiceModel(IAuthorizationService authorization)
        {
            _authorization = authorization;
        }
        
        public string Message { get; set; }

        public async Task OnGet()
        {
            var invoice = new Invoice()
            {
                MandantId = 2,
                Value = 4711m,
                Id = 1312344
            };

            var result = await _authorization.AuthorizeAsync(User, invoice, Operation.Read);
            if (result.Succeeded)
            {
                this.Invoice = invoice;
                Message = "Deine Rechnung";

            }
            else
            {
                Message = "Diese Rechnung existiert nicht";
            }
        }

        public Invoice Invoice { get; set; }
    }
}
