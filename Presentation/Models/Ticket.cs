namespace Presentation.Models;

public class Ticket
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EventId { get; set; } = null!;
    public string PackageId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;

}
