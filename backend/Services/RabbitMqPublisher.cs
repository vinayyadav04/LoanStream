using LoanStream.Api.Events;

namespace LoanStream.Api.Services;

public sealed class RabbitMqPublisher : IRabbitMqPublisher
{
    public Task PublishLeadCreatedAsync(LeadCreatedEvent leadCreatedEvent)
    {
        return Task.CompletedTask;
    }
}
