using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddDbContext<TicketDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITicketService, TicketService>();

var serviceBusConnection = builder.Configuration["ServiceBusConnection"];
builder.Services.AddSingleton(x => new ServiceBusClient(serviceBusConnection));
builder.Services.AddSingleton(x =>
    x.GetRequiredService<ServiceBusClient>().CreateSender("event-bus"));
builder.Services.AddSingleton<TicketBusListener>();
builder.Services.AddHostedService<TicketBusListener>();



var app = builder.Build();
app.MapOpenApi();
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run();
