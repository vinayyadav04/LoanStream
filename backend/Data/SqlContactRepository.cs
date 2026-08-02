using LoanStream.Api.Models;
using LoanStream.Api.Services;
using Npgsql;

namespace LoanStream.Api.Data;

public sealed class SqlContactRepository : IContactRepository
{
    private readonly string _connectionString;

    public SqlContactRepository(AppSettings settings)
    {
        _connectionString = string.IsNullOrWhiteSpace(settings.DatabaseConnectionString)
            ? settings.SqlServerConnectionString
            : settings.DatabaseConnectionString;
    }

    public async Task<int> InsertAsync(ContactRecord contact)
    {
        try
        {
            await EnsureSchemaAsync();
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO Contacts (Name, Email, Phone, Topic, Message, CreatedDate)
            VALUES (@Name, @Email, @Phone, @Topic, @Message, @CreatedDate)
            RETURNING Id;";
            command.Parameters.AddWithValue("@Name", contact.Name);
            command.Parameters.AddWithValue("@Email", contact.Email);
            command.Parameters.AddWithValue("@Phone", contact.Phone);
            command.Parameters.AddWithValue("@Topic", contact.Topic);
            command.Parameters.AddWithValue("@Message", contact.Message);
            command.Parameters.AddWithValue("@CreatedDate", contact.CreatedDate);
            var result = await command.ExecuteScalarAsync();
            return result is int id ? id : 0;
        }
        catch (NpgsqlException)
        {
            return 0;
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
            CREATE TABLE IF NOT EXISTS Contacts (
                Id SERIAL PRIMARY KEY,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL,
                Phone TEXT NOT NULL,
                Topic TEXT NULL,
                Message TEXT NOT NULL,
                CreatedDate TIMESTAMPTZ NOT NULL
            );";
            await command.ExecuteNonQueryAsync();
        }
        catch (NpgsqlException)
        {
            // Ignore schema errors when the database is unavailable.
        }
    }
}
