using Presentation.Models;

namespace Presentation.Services
{
    public interface ITicketService
    {
        Task<ServiceResponse<Ticket?>> CreateTicketAsync(TicketRegistrationDto ticket);
        Task<ServiceResponse<bool>> DeleteTicketAsync(string id);
        Task<ServiceResponse<List<Ticket>>> GetAllTicketsForPackageAsync(string packageId);
        Task<ServiceResponse<Ticket?>> GetTicketAsync(string id);
        Task<ServiceResponse<Ticket?>> UpdateTicketAsync(Ticket ticket);
    }
}