using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController(ITicketService ticketService) : ControllerBase
    {
        private readonly ITicketService _ticketService = ticketService;

        [HttpGet]
        public async Task<IActionResult> GetAllTicketsAsync()
        {
            var response = await _ticketService.GetAllTicketsAsync();
            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("forpackage/{packageId}")]
        public async Task<IActionResult> GetAllTicketsForPackage(string packageId)
        {
            if (string.IsNullOrEmpty(packageId))
            {
                return BadRequest("Package ID cannot be null or empty.");
            }
            var response = await _ticketService.GetAllTicketsForPackageAsync(packageId);
            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Ticket ID cannot be null or empty.");
            }
            var response = await _ticketService.GetTicketAsync(id);
            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("sold/{eventId}")]
        public async Task<IActionResult> GetSoldTicketsForEvent(string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                return BadRequest("Event ID cannot be null or empty.");
            }
            var response = await _ticketService.GetTicketsSoldAsync(eventId);
            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTickets([FromBody] TicketRegistrationDto tickets)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _ticketService.CreateTicketsAsync(tickets);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return CreatedAtAction(nameof(CreateTickets), new { success = response.Success }, response.Data);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(string id, [FromBody] Ticket ticket)
        {
            if (string.IsNullOrEmpty(id) || ticket == null || ticket.Id != id)
            {
                return BadRequest("Invalid ticket data.");
            }
            var response = await _ticketService.UpdateTicketAsync(ticket);
            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Data);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Ticket ID cannot be null or empty.");
            }
            var response = await _ticketService.DeleteTicketAsync(id);
            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            return NoContent();
        }
    }
}
