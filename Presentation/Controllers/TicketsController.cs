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


        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] TicketRegistrationDto ticket)
        {
            if (ticket == null)
            {
                return BadRequest("Ticket data cannot be null.");
            }
            var response = await _ticketService.CreateTicketAsync(ticket);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return CreatedAtAction(nameof(GetTicket), new { id = response.Data?.Id }, response.Data);
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
