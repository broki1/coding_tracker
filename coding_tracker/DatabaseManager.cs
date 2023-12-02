
using Microsoft.Data.Sqlite;

namespace coding_tracker;

internal class DatabaseManager
{
    internal static void CreateTable(string ConnectionString)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding(
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Date TEXT,
                                            StartTime TEXT,
                                            EndTime TEXT,
                                            Duration TEXT
                                         )";

            createTableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}