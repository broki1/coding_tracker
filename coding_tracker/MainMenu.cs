
using System.Configuration;
using System.Globalization;

namespace coding_tracker
{
    internal class MainMenu
    {

        static CodingController codingController = new CodingController();

        internal static void Start()
        {
            bool closeApp = false;

            while (!closeApp)
            {
                Console.Clear();
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Welcome to the Coding Tracker app!");
                Console.WriteLine("-------------------------------------------\n\n");

                Console.WriteLine("Enter 0 to exit the program.");
                Console.WriteLine("Enter 1 to view record.");
                Console.WriteLine("Enter 2 to add record.");
                Console.WriteLine("Enter 3 to update record.");
                Console.WriteLine("Enter 4 to delete record.\n\n");

                var userInput = Console.ReadLine().Trim();

                while (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out _))
                {
                    Console.WriteLine("\nInvalid input. Please enter a number from 0 to 4.\n");
                    userInput = Console.ReadLine().Trim();
                }

                switch (userInput)
                {
                    case "0":
                        Environment.Exit(0);
                        closeApp = true;
                        break;
                    case "1":
                        codingController.Get();
                        break;
                    case "2":
                        MainMenu.ProcessAdd();
                        break;
                    case "3":
                        MainMenu.ProcessUpdate();
                        break;
                    case "4":
                        MainMenu.ProcessDelete();
                        break;
                    default:
                        Console.WriteLine("\nInvalid input. Please enter a number from 0 to 4.\n");
                        break;
                }
            }

        }

        private static void ProcessDelete()
        {
            codingController.Get();

            Console.WriteLine("\n\nPlease enter the ID of the record you want to delete.");
            var id = Console.ReadLine().Trim();

            while (!int.TryParse(id, out _) || !codingController.CheckId(int.Parse(id)))
            {
                Console.WriteLine("\n\nInvalid input. Please enter the ID of the record you want to delete.");
                id = Console.ReadLine();
            }

            codingController.Delete(int.Parse(id));
        }

        private static void ProcessUpdate()
        {
            codingController.Get();

            Console.WriteLine("\n\nPlease enter the ID of the record you want to update.");
            var id = Console.ReadLine().Trim();

            while (!int.TryParse(id, out _) || !codingController.CheckId(int.Parse(id)))
            {
                Console.WriteLine("\n\nInvalid input. Please enter the ID of the record you want to update.");
                id = Console.ReadLine();
            }

            var date = MainMenu.GetDateInput();
            var startTime = MainMenu.GetTime("start");
            var endTime = MainMenu.GetTime("end");

            var duration = MainMenu.CalculateDuration(startTime, endTime);

            CodingSession codingSession = new CodingSession();

            codingSession.Date = date;
            codingSession.StartTime = startTime;
            codingSession.EndTime = endTime;
            codingSession.Duration = duration.ToString();

            codingController.Update(codingSession, int.Parse(id));
        }

        private static void ProcessAdd()
        {
            var date = MainMenu.GetDateInput();
            var startTime = MainMenu.GetTime("start");
            var endTime = MainMenu.GetTime("end");

            var duration = MainMenu.CalculateDuration(startTime, endTime);

            CodingSession codingSession = new CodingSession();

            codingSession.Date = date;
            codingSession.StartTime = startTime;
            codingSession.EndTime = endTime;
            codingSession.Duration = duration.ToString();

            codingController.Post(codingSession);
        }

        private static string GetTime(string startOrEnd)
        {
            Console.WriteLine($"Please enter the {startOrEnd} time: (Format (24-hour): hh:mm). Type 0 to return to the main menu.");
            var time = Console.ReadLine().Trim();

            if (time == "0") MainMenu.Start();

            while (!DateTime.TryParseExact(time, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid input. Please enter the start time in (HH:mm) format.");
                time = Console.ReadLine().Trim();
            }

            return time;
        }

        private static TimeSpan CalculateDuration(string startTime, string endTime)
        {
            TimeSpan duration = DateTime.Parse(endTime) - DateTime.Parse(startTime);

            if (duration <= TimeSpan.Zero)
            {
                duration = duration.Add(TimeSpan.FromDays(1));
                Console.WriteLine(duration.ToString());
            }

            return duration;
        }



        private static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-MM-yy). Type 0 to return to the main menu.\n\n");

            var dateInput = Console.ReadLine().Trim();

            if (dateInput == "0") MainMenu.Start();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nNot a valid date. Please enter date in the format: dd-MM-yy\n\n");

                dateInput = Console.ReadLine().Trim();

                if (dateInput == "0") MainMenu.Start();
            }

            return dateInput;
        }
    }
}