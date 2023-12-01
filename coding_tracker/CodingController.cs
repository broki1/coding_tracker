using Microsoft.Data.Sqlite;
using System.Configuration;

namespace coding_tracker;

internal class CodingController
{
    string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    internal void Get()
    {
        Console.Clear();

        List<CodingSession> tableData = new List<CodingSession>();

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText = "SELECT * FROM coding";

                var reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("ID\t\tDate\t\t\tDuration");
                    Console.WriteLine("---------------------------------------------");

                    while (reader.Read())
                    {
                        tableData.Add(new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetString(1),
                            Duration = reader.GetString(2)
                        });
                    }
                } else
                {
                    Console.WriteLine("\n\nNo rows found.");
                }

                reader.Close();
            }
        }

        Console.WriteLine("\n\n");

        Console.WriteLine("\n\nPress any key to continue.");
        Console.ReadKey();
    }

    internal void Post(CodingSession codingSession)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO coding (Date, Duration) VALUES ('{codingSession.Date}', '{codingSession.Duration}')";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}