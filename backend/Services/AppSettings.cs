namespace LoanStream.Api.Services;

public sealed class AppSettings
{
    public string DatabaseConnectionString { get; set; } = string.Empty;
    public string SqlServerConnectionString { get; set; } = string.Empty;
    public string RabbitMqConnectionString { get; set; } = string.Empty;
}
