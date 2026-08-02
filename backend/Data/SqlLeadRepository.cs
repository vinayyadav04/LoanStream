using System.Collections.Concurrent;
using LoanStream.Api.Models;
using LoanStream.Api.Services;
using Npgsql;
using Microsoft.Extensions.Logging;

namespace LoanStream.Api.Data;

public sealed class SqlLeadRepository : ILeadRepository
{
    private static readonly ConcurrentDictionary<int, LeadRecord> _fallbackLeads = new();
    private readonly string _connectionString;
    private readonly ILogger<SqlLeadRepository> _logger;

    public SqlLeadRepository(AppSettings settings, ILogger<SqlLeadRepository> logger)
{
    _logger = logger;

    _connectionString = string.IsNullOrWhiteSpace(settings.DatabaseConnectionString)
        ? settings.SqlServerConnectionString
        : settings.DatabaseConnectionString;

    _logger.LogInformation("Raw Connection String: {ConnectionString}", _connectionString);

    try
    {
        var builder = new Npgsql.NpgsqlConnectionStringBuilder(_connectionString);

        _logger.LogInformation(
            "Host={Host}, Port={Port}, Database={Database}, Username={Username}",
            builder.Host,
            builder.Port,
            builder.Database,
            builder.Username);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Invalid connection string format");
    }
}

    public async Task<Guid> InsertAsync(LeadRecord lead)
    {
        try
        {
            await EnsureSchemaAsync();
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO Leads (Name, Email, Phone, EmploymentType, MonthlyIncome, LoanAmount, City, Source, CreatedDate, Status)
            VALUES (@Name, @Email, @Phone, @EmploymentType, @MonthlyIncome, @LoanAmount, @City, @Source, @CreatedDate, @Status)
            RETURNING Id;";
            command.Parameters.AddWithValue("@Name", lead.Name);
            command.Parameters.AddWithValue("@Email", lead.Email);
            command.Parameters.AddWithValue("@Phone", lead.Phone);
            command.Parameters.AddWithValue("@EmploymentType", lead.EmploymentType);
            command.Parameters.AddWithValue("@MonthlyIncome", lead.MonthlyIncome);
            command.Parameters.AddWithValue("@LoanAmount", lead.LoanAmount);
            command.Parameters.AddWithValue("@City", lead.City);
            command.Parameters.AddWithValue("@Source", lead.Source);
            command.Parameters.AddWithValue("@CreatedDate", lead.CreatedDate);
            command.Parameters.AddWithValue("@Status", lead.Status);
            var result = await command.ExecuteScalarAsync();

            return result is Guid id
                ? id
                : Guid.Empty;
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError(ex, "Failed to save lead to Postgres using connection string {ConnectionString}", _connectionString);
            throw;
        }
    }

    public async Task<IReadOnlyList<LeadRecord>> GetAllAsync(string? name = null, string? phone = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        try
        {
            await EnsureSchemaAsync();
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT Id, Name, Email, Phone, EmploymentType, MonthlyIncome, LoanAmount, City, Source, CreatedDate, Status
            FROM leads
            WHERE (@Name IS NULL OR Name ILIKE '%' || @Name || '%')
              AND (@Phone IS NULL OR Phone ILIKE '%' || @Phone || '%')
              AND (@FromDate IS NULL OR CreatedDate >= @FromDate)
              AND (@ToDate IS NULL OR CreatedDate <= @ToDate)
            ORDER BY CreatedDate DESC;";
            command.Parameters.AddWithValue("@Name", name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Phone", phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@FromDate", fromDate ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ToDate", toDate ?? (object)DBNull.Value);

            await using var reader = await command.ExecuteReaderAsync();
            var result = new List<LeadRecord>();
            while (await reader.ReadAsync())
            {
                _logger.LogInformation("Lead Found: {Name}", reader.GetString(1));

                result.Add(new LeadRecord
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Phone = reader.GetString(3),
                    EmploymentType = reader.GetString(4),
                    MonthlyIncome = reader.GetString(5),
                    LoanAmount = reader.GetFloat(6),
                    City = reader.GetString(7),
                    Source = reader.GetString(8),
                    CreatedDate = reader.GetDateTime(9),
                    Status = reader.GetString(10)
                });
            }

            _logger.LogInformation("Total Leads: {Count}", result.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllAsync failed");
            throw;
        }
    }

    private async Task EnsureSchemaAsync()
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Leads (
            Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
            Name VARCHAR(255),
            Email VARCHAR(255),
            Phone VARCHAR(50),
            EmploymentType VARCHAR(100),
            MonthlyIncome REAL,
            LoanAmount REAL,
            City VARCHAR(100),
            Source VARCHAR(100),
            CreatedDate DATE,
            Status VARCHAR(50)
        );";
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Insert failed");
            throw;
        }
    }
}
