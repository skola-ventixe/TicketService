using Presentation.Models;

namespace Presentation.Services
{
    public interface ITicketService
    {
        Task<ServiceResponse<List<Ticket?>>> CreateTicketsAsync(TicketRegistrationDto ticket);
        Task<ServiceResponse<bool>> DeleteTicketAsync(string id);
        Task<ServiceResponse<IEnumerable<Ticket>>> GetAllTicketsAsync();
        Task<ServiceResponse<List<Ticket>>> GetAllTicketsForPackageAsync(string packageId);
        Task<ServiceResponse<Ticket?>> GetTicketAsync(string id);
        Task<ServiceResponse<int>> GetTicketsSoldAsync(string eventId);
        Task<ServiceResponse<Ticket?>> UpdateTicketAsync(Ticket ticket);
    }
}