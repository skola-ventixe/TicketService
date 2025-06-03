namespace Presentation.Models;

public class TicketRegistrationDto
{
    public string EventId { get; set; } = null!;
    public string PackageId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public List<string> Names { get; set; } = [];
}
