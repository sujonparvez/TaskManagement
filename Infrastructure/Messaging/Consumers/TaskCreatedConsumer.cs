using Application.DTOs;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Consumers
{
    public class TaskCreatedConsumer(ILogger<TaskCreatedConsumer> _logger) : IConsumer<TaskCreatedMessage>
    {
        public async Task Consume(ConsumeContext<TaskCreatedMessage> context)
        {
            _logger.LogInformation($"Task Created :Id= {context.Message.Task.Id}, Title={context.Message.Task.Title}");
            await Task.CompletedTask;
        }
    }
    public class TaskUpdatedConsumer(ILogger<TaskUpdatedConsumer> _logger) : IConsumer<TaskUpdatedMessage>
    {
        public async Task Consume(ConsumeContext<TaskUpdatedMessage> context)
        {
            _logger.LogInformation($"Task Updated :Id= {context.Message.Task.Id}, Title={context.Message.Task.Title}");
            await Task.CompletedTask;
        }
    }
}
