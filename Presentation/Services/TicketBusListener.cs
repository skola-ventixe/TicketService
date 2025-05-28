
using Azure.Messaging.ServiceBus;
using Presentation.Models;
using System.Text.Json;

namespace Presentation.Services;

public class TicketBusListener : BackgroundService
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ServiceBusSender _eventBusSender;
    private readonly IServiceScopeFactory _scopeFactory;

    public TicketBusListener(ServiceBusClient client, IServiceScopeFactory scopeFactory, ServiceBusSender eventBusSender)
    {
        _client = client;
        _eventBusSender = eventBusSender;
        _scopeFactory = scopeFactory;
        _processor = _client.CreateProcessor("ticket-bus", new ServiceBusProcessorOptions());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += ProcessMessageHandler;
        _processor.ProcessErrorAsync += ProcessErrorHandler;
        await _processor.StartProcessingAsync(stoppingToken);
    }

    private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
    {
        using var scope = _scopeFactory.CreateScope();
        var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();

        var message = args.Message;
        var body = message.Body.ToString();
        var eventType = message.ApplicationProperties["EventType"].ToString();
        string responseMessage;
        ServiceBusMessage response;

        switch (eventType)
        {
            case "Add":
                var ticketToAdd = JsonSerializer.Deserialize<TicketRegistrationDto>(body);
                if (ticketToAdd != null)
                {
                    var addResponse = await ticketService.CreateTicketAsync(ticketToAdd);
                    if (!addResponse.Success)
                    {
                        throw new Exception(addResponse.Error);
                    }
                    responseMessage = JsonSerializer.Serialize(addResponse);
                }
                else
                {
                    responseMessage = JsonSerializer.Serialize(new ServiceResponse<bool>
                    {
                        Success = false,
                        Error = "Package to add is null."
                    });

                }
                response = new ServiceBusMessage(responseMessage)
                {
                    ApplicationProperties =
                    {
                        ["EventType"] = "AddResponse",
                        ["CorrelationId"] = message.ApplicationProperties["CorrelationId"]
                    }
                };
                await _eventBusSender.SendMessageAsync(response);
                break;


            case "GetPackageForEvent":
                break;


            case "Update":

                break;


            case "Delete":

                break;


            default:
                throw new ArgumentException($"Unknown event type: {eventType}");
        }
        await args.CompleteMessageAsync(message);
    }

    private Task ProcessErrorHandler(ProcessErrorEventArgs args)
    {
        // Handle the error
        Console.WriteLine($"Error processing message: {args.Exception}");
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        await _processor.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}

