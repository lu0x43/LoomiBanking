using Loomi.Transactions.Application.Messages;
using MassTransit;

namespace Loomi.Clients.Api.Consumers;

public class TransferCompletedConsumer : IConsumer<TransferCompletedEvent>
{
    private readonly ILogger<TransferCompletedConsumer> _logger;

    public TransferCompletedConsumer(ILogger<TransferCompletedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TransferCompletedEvent> context)
    {
        var message = context.Message;

        // Simulação da notificação exigida pelo PDF usando o RabbitMQ
        _logger.LogInformation(
            $"[RABBITMQ] Notificação: A transação {message.TransactionId} de R$ {message.Amount} foi processada com sucesso.");

        return Task.CompletedTask;
    }
}