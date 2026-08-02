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
        _connectionString = string.IsNullOrWhiteSpace(settings.DatabaseConnectionString)
            ? settings.SqlServerConnectionString
            : settings.DatabaseConnectionString;
        _logger = logger;
    }

    public async Task<int> InsertAsync(LeadRecord lead)
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
            return result is int id ? id : 0;
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError(ex, "Failed to save lead to Postgres using connection string {ConnectionString}", _connectionString);
            return 0;
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
            FROM Leads
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
                result.Add(new LeadRecord
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Phone = reader.GetString(3),
                    EmploymentType = reader.GetString(4),
                    MonthlyIncome = reader.GetString(5),
                    LoanAmount = reader.GetDecimal(6),
                    City = reader.GetString(7),
                    Source = reader.GetString(8),
                    CreatedDate = reader.GetDateTime(9),
                    Status = reader.GetString(10)
                });
            }

            return result;
        }
        catch (NpgsqlException)
        {
            return _fallbackLeads.Values
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
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
                Id SERIAL PRIMARY KEY,
                Name TEXT NOT NULL,
                Email TEXT NULL,
                Phone TEXT NOT NULL,
                EmploymentType TEXT NULL,
                MonthlyIncome TEXT NULL,
                LoanAmount DECIMAL(12,2) NOT NULL,
                City TEXT NULL,
                Source TEXT NULL,
                CreatedDate TIMESTAMPTZ NOT NULL,
                Status TEXT NOT NULL
            );";
            await command.ExecuteNonQueryAsync();
        }
        catch (NpgsqlException)
        {
            // Ignore schema errors when the database is unavailable.
        }
    }
}
