using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.Security
{
    public class Operation
    {
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement() {Name = "read"};
        public static OperationAuthorizationRequirement Write = new OperationAuthorizationRequirement() {Name = "write"};
    }
    public class InvoiceAuthorizationRequirementHandler :AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {
        private readonly IServiceProvider _serviceProvider;

        public InvoiceAuthorizationRequirementHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Invoice invoice)
        {
            var authorizationService = _serviceProvider.GetRequiredService<IAuthorizationService>();

            AuthorizationResult result;
            
            if (requirement.Name == "read")
            {
                result = await authorizationService.AuthorizeAsync(context.User, "InvoiceReader");
            }
            else if (requirement.Name == "write")
            {
                result = await authorizationService.AuthorizeAsync(context.User, "InvoiceWriter");
            }
            else
            {
                return;
            }
            
            if (result.Succeeded)
            {
                var mandantId = context.User.FindFirst("mandant_id");
                if (string.IsNullOrWhiteSpace(mandantId?.Value))
                {
                    return;
                }
                if (mandantId.Value == invoice.MandantId.ToString(CultureInfo.InvariantCulture))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}