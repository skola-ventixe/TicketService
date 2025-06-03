using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Models;

namespace Presentation.Services;

public class TicketService : ITicketService
{
    private readonly TicketDataContext _context;
    private readonly DbSet<Ticket> _tickets;

    public TicketService(TicketDataContext context)
    {
        _context = context;
        _tickets = _context.Set<Ticket>();
    }

    public async Task<ServiceResponse<IEnumerable<Ticket>>> GetAllTicketsAsync()
    {
        var allTickets = await _tickets.ToListAsync();
        if (allTickets == null)
        {
            return new ServiceResponse<IEnumerable<Ticket>>
            {
                Success = false,
                Message = "No tickets could be found.",
                Data = []
            };
        }

        return new ServiceResponse<IEnumerable<Ticket>>
        {
            Success = true,
            Data = allTickets
        };
    }

    public async Task<ServiceResponse<Ticket?>> GetTicketAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return new ServiceResponse<Ticket?>
            {
                Success = false,
                Message = "Ticket ID cannot be null or empty."
            };
        try
        {
            var ticket = await _tickets.FindAsync(id);
            if (ticket == null)
            {
                return new ServiceResponse<Ticket?>
                {
                    Success = false,
                    Message = "Ticket not found."
                };
            }
            return new ServiceResponse<Ticket?>
            {
                Success = true,
                Data = ticket
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<Ticket?>
            {
                Success = false,
                Message = "An error occurred while retrieving the ticket.",
                Error = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<List<Ticket>>> GetAllTicketsForPackageAsync(string packageId)
    {
        if (string.IsNullOrEmpty(packageId))
            return new ServiceResponse<List<Ticket>>
            {
                Success = false,
                Message = "Event ID cannot be null or empty."
            };
        try
        {
            var tickets = await _tickets
                .Where(t => t.PackageId == packageId)
                .ToListAsync();
            if (tickets == null || tickets.Count == 0)
            {
                return new ServiceResponse<List<Ticket>>
                {
                    Success = false,
                    Message = "No tickets found for the specified package."
                };
            }
            return new ServiceResponse<List<Ticket>>
            {
                Success = true,
                Data = tickets
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<List<Ticket>>
            {
                Success = false,
                Message = "An error occurred while retrieving tickets for the package.",
                Error = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<int>> GetTicketsSoldAsync(string eventId)
    {
        if (string.IsNullOrEmpty(eventId))
            return new ServiceResponse<int>
            {
                Success = false,
                Message = "Event ID cannot be null or empty."
            };
        try
        {
            var ticketsSold = await _tickets
                .CountAsync(t => t.EventId == eventId);
            return new ServiceResponse<int>
            {
                Success = true,
                Data = ticketsSold
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<int>
            {
                Success = false,
                Message = "An error occurred while counting the tickets sold.",
                Error = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<List<Ticket?>>> CreateTicketsAsync(TicketRegistrationDto tickets)
    {
        if (tickets == null)
            return new ServiceResponse<List<Ticket?>>
            {
                Success = false,
                Message = "Ticket registration data cannot be null."
            };
        try
        {
            List<Ticket> createdTickets = [];

            foreach(var name in tickets.Names)
            {
                var entity = new Ticket
                {
                    PackageId = tickets.PackageId,
                    UserId = tickets.UserId,
                    Name = name
                };
                _tickets.Add(entity);
                createdTickets.Add(entity);
            }
            await _context.SaveChangesAsync();
            return new ServiceResponse<List<Ticket?>>
            {
                Success = true,
                Message = "Ticket created successfully.",
                Data = createdTickets!
            };

        }
        catch (Exception ex)
        {
            return new ServiceResponse<List<Ticket?>>
            {
                Success = false,
                Message = "An error occurred while creating the ticket.",
                Error = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<Ticket?>> UpdateTicketAsync(Ticket ticket)
    {
        if (ticket == null)
            return new ServiceResponse<Ticket?>
            {
                Success = false,
                Message = "Ticket data cannot be null."
            };
        try
        {
            _tickets.Update(ticket);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Ticket?>
            {
                Success = true,
                Message = "Ticket updated successfully.",
                Data = ticket
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<Ticket?>
            {
                Success = false,
                Message = "An error occurred while updating the ticket.",
                Error = ex.Message
            };
        }
    }

    public Task<ServiceResponse<bool>> DeleteTicketAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return Task.FromResult(new ServiceResponse<bool>
            {
                Success = false,
                Message = "Ticket ID cannot be null or empty."
            });
        try
        {
            var ticket = _tickets.Find(id);
            if (ticket == null)
            {
                return Task.FromResult(new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Ticket not found."
                });
            }
            _tickets.Remove(ticket);
            _context.SaveChanges();
            return Task.FromResult(new ServiceResponse<bool>
            {
                Success = true,
                Message = "Ticket deleted successfully.",
                Data = true
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ServiceResponse<bool>
            {
                Success = false,
                Message = "An error occurred while deleting the ticket.",
                Error = ex.Message
            });
        }
    }
}
