using System.Threading.Tasks;

namespace Airslip.Common.Services.Handoff.Interfaces;

public interface IMessageHandoffService
{
    Task ProcessMessage(string triggerName, string message);
}