using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Types
{
    public interface IEmailSender
    {
        Task<EmailOutcome> SendEmail(
            IEnumerable<EmailAddressRecipient> emailAddressTos,
            string subject,
            string plainTextContent,
            string htmlContent,
            IFormFileCollection? attachments = null);
    }
}