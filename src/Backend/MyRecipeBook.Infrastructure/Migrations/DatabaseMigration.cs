using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastructure.Migrations
{
    public static class DatabaseMigration
    {
        public static void Migrate(string connectionString, IServiceProvider serviceProvider)
        {
            EnsureDatabaseCreated(connectionString);
            MigrationDatabase(serviceProvider);
        }

        static void EnsureDatabaseCreated(string connectionString)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            var databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Remove("Database");
            using (var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString))
            {
                DynamicParameters parameters = new();
                parameters.Add("name", databaseName);
                var records = dbConnection.Query("select * from Information_schema.Schemata where schema_Name = @name", parameters);
                if (!records.Any())
                {
                    dbConnection.Execute($"Create Database {databaseName}");
                }                
            }
        }

        static void MigrationDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.ListMigrations();
            runner.MigrateUp();
        }
    }
}
