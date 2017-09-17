using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataProtection.Pages
{
    public class DecryptModel : PageModel
    {
        private readonly IDataProtectionProvider _dataProtection;

        public DecryptModel(IDataProtectionProvider dataProtection)
        {
            _dataProtection = dataProtection;
        }

        public async Task OnGetAsync()
        {
            try
            {
                var bytes = await System.IO.File.ReadAllBytesAsync("protected.bytes");
                var protector = _dataProtection.CreateProtector("TextFromPage"); // Bereich festlegen
                protector = protector.CreateProtector(User.Identity.Name); // nur für den User entschlüsselbar
                bytes = protector.Unprotect(bytes);
                Message = Encoding.UTF8.GetString(bytes);
            }
            catch (CryptographicException e)
            {
                Message = "Die Nachricht konnte nicht gelesen werden";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

        }

        public string Message { get; set; }

    }

}