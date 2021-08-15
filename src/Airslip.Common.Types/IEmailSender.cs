using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Types
{
    public interface IEmailSender
    {
        Task<EmailOutcome> SendEmail(
            IEnumerable<string> to,
            string subject,
            string content,
            IFormFileCollection? attachments = null);
    }
}