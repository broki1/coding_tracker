using ConsoleTableExt;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace coding_tracker;

internal class CodingController
{
    string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    string todaysDate = DateTime.Now.Date.ToString("yyyy-MM-dd");

    internal void Get(string timeFrame = "")
    {
        Console.Clear();

        List<CodingSession> tableData = new List<CodingSession>();

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText = "SELECT * FROM coding";

                if (!string.IsNullOrEmpty(timeFrame))
                {
                    tableCmd.CommandText += $" WHERE date between '{timeFrame}' and '{todaysDate}'";
                }

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
    }

    internal void Post(CodingSession codingSession)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO coding (Date, StartTime, EndTime, Duration) VALUES ('{codingSession.Date}', '{codingSession.StartTime}', '{codingSession.EndTime}', '{codingSession.Duration}')";

            tableCmd.ExecuteNonQuery();
        }
    }

    internal void Update(CodingSession codingSession, int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @$"UPDATE coding SET Date = '{codingSession.Date}',
                                      StartTime = '{codingSession.StartTime}',
                                      EndTime = '{codingSession.EndTime}',
                                      Duration = '{codingSession.Duration}'
                                      WHERE Id = {id}";

            tableCmd.ExecuteNonQuery();
        }
    }

    internal bool CheckId(int id)
    {
        bool exists = false;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM coding WHERE Id = {id})";
            
            var result = Convert.ToInt32(tableCmd.ExecuteScalar());

            if (result == 1)
            {
                exists = true;
            }
        }

        return exists;
    }

    internal void Delete(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM coding WHERE Id = {id}";
            tableCmd.ExecuteNonQuery();
        }
    }
}