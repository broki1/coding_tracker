
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
                        break;
                    case "4":
                        break;
                    default:
                        Console.WriteLine("\nInvalid input. Please enter a number from 0 to 4.\n");
                        break;
                }
            }

        }

        private static void ProcessAdd()
        {
            var date = MainMenu.GetDateInput();

            var duration = MainMenu.GetDurationInput();

            CodingSession codingSession = new CodingSession();

            codingSession.Date = date;
            codingSession.Duration = duration;

            codingController.Post(codingSession);
        }

        private static string GetDurationInput()
        {
            Console.WriteLine("Please enter the duration: (Format: hh:mm). Type 0 to return to the main menu.");

            var durationInput = Console.ReadLine().Trim();

            if (durationInput == "0") MainMenu.Start();

            while (!TimeSpan.TryParseExact(durationInput, "h\\:mm", CultureInfo.InvariantCulture, out _))
            {
                Console.WriteLine("\n\nNot a valid duration. Please enter duration in the format: hh:mm\n\n");

                durationInput = Console.ReadLine().Trim();

                if (durationInput == "0") MainMenu.Start();
            }

            return durationInput;
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