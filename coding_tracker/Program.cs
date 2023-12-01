using System.Configuration;

namespace coding_tracker;

internal class Program
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    static void Main(string[] args)
    {
        DatabaseManager.CreateTable(connectionString);

        MainMenu.Start();
    }
}
