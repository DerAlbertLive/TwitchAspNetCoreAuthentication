using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataProtection.Pages
{
    public class EncryptModel : PageModel
    {
        private readonly IDataProtectionProvider _dataProtection;

        public EncryptModel(IDataProtectionProvider dataProtection)
        {
            _dataProtection = dataProtection;
        }
        
        [BindProperty]
        public InputModel Input { get; set; }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var protector = _dataProtection.CreateProtector("TextFromPage"); // Bereich festlegen
            protector = protector.CreateProtector(User.Identity.Name); // nur für den User entschlüsselbar
            var bytes = protector.Protect(Encoding.UTF8.GetBytes(Input.Text));
            await System.IO.File.WriteAllBytesAsync("protected.bytes", bytes);
            return Page();
        }
    }

    public class InputModel
    {
        public string Text { get; set; }
    }
}