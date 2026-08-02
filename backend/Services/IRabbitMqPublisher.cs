using LoanStream.Api.Events;

namespace LoanStream.Api.Services;

public interface IRabbitMqPublisher
{
    Task PublishLeadCreatedAsync(LeadCreatedEvent leadCreatedEvent);
}
