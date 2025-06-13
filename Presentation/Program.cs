using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Services;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

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
    options.UseSqlServer(builder.Configuration.GetValue<string>("SqlServer")));

builder.Services.AddScoped<ITicketService, TicketService>();

var app = builder.Build();
app.MapOpenApi();
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run();
