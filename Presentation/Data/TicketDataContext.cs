using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace Presentation.Data;

public class TicketDataContext(DbContextOptions<TicketDataContext> options) : DbContext(options)
{
    public DbSet<Ticket> Tickets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
