using ConsoleTableExt;
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

                    while (reader.Read())
                    {
                        tableData.Add(new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetString(1),
                            StartTime = reader.GetString(2),
                            EndTime = reader.GetString(3),
                            Duration = reader.GetString(4)
                        });
                    }
                } else
                {
                    Console.WriteLine("\n\nNo rows found.");
                }

                reader.Close();
            }
        }

        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("\tCoding Tracker Table");
        Console.WriteLine("-----------------------------------------");

        Console.WriteLine("\n\n");

        ConsoleTableBuilder.From(tableData).ExportAndWriteLine();

        Console.WriteLine("\n\nPress any key to continue.");
        Console.ReadKey();
    }

    internal void Post(CodingSession codingSession)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO coding (Date, StartTime, EndTime, Duration) VALUES ('{codingSession.Date}', '{codingSession.StartTime}', '{codingSession.EndTime}', '{codingSession.Duration}')";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}