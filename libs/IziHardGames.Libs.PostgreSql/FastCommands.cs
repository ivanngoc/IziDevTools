using System.Threading.Tasks;
using Npgsql;

namespace IziHardGames.DevTools.ForPostgreSql;

public class FastCommands
{
    private readonly string connectionString;
    private readonly NpgsqlDataSource dataSource;

    public FastCommands(string connectionString)
    {
        this.connectionString = connectionString;
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        this.dataSource = dataSourceBuilder.Build();
    }

    public async Task ClearAllTablesAsync()
    {
        await dataSource.OpenConnectionAsync();
        var truncateCommand = @"
                DO $$ DECLARE
                r RECORD;
                BEGIN
                    FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = current_schema()) LOOP
                        EXECUTE 'TRUNCATE TABLE' || quote_ident(r.tablename) || 'CASCADE';
                    END LOOP;
                END $$;";
        var command = dataSource.CreateCommand(truncateCommand);
        await command.ExecuteNonQueryAsync();
    }
}