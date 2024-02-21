using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Sim
{
    internal class AtmFunctions
    {
        private static string filePath = "D:\\IT_STEP\\FinalProjects\\Atm_Users";

        public static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Sabank ATM!");
            Console.WriteLine("1. Enter ID\t | 2. Add User\t | 3. Exit");

            int choice;
            bool validChoice = false;

            do
            {
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    validChoice = true;
                    switch (choice)
                    {
                        case 1:
                            SearchByID();
                            break;
                        case 2:
                            AddUser();
                            break;
                        case 3:
                            Console.WriteLine("Exiting program.");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid option.");
                            validChoice = false;
                            MainMenu();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                }
            }
            while (!validChoice);
        }

        #region Case 1: Log In
        public static void SearchByID()
        {
            Console.Write("Enter User ID: ");
            if (long.TryParse(Console.ReadLine(), out long userID))
            {
                string userFullName = FindUser(userID);
                //თუ FindUser ფუნქციამ არ დააბრუნა null მაშინ userMenuUi ფუნქცია გაეშვას და გადაეცეს იუზერის სახელი
                if (userFullName != null)
                {
                    UserMenuUi(userID, userFullName);
                }
                else
                {
                    //თუ FindUser ფუნქციამ დააბრუნა null მაშინ გამოიტანოს შემდეგი
                    Console.WriteLine("User not found.");
                    Console.WriteLine("Would you like to search again? (Y/N)");
                    string response;
                    do
                    {
                        response = Console.ReadLine().ToUpper();
                        if (response == "N")
                        {
                            Console.Clear();
                            MainMenu();
                            return;
                        }
                        else if (response != "Y")
                        {
                            Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                        }
                    } while (response != "Y");
                    SearchByID();
                }
            }
        }
        public static string FindUser(long userID)
        {
            string fileName = filePath + "\\User" + userID.ToString() + ".txt";
            if (File.Exists(fileName))
            {
                // წავიკთხოთ მომხმარებლის სახელი ფაილიდან
                using (StreamReader reader = File.OpenText(fileName))
                {
                    // პრეფიქსის გამოტოვება
                    return reader.ReadLine()?.Substring(6);
                }
            }
            else
            {
                return null; // მომხმარებლი არ  მოიძებნა
            }
        }

        #endregion

        #region Case 2: Add User
        public static void AddUser()
        {
            long userIDNumber;
            string userFullName;
            bool validUserID = false;

            //სანამ ვალიდური არ იქნება UserID მანამ იმუშაოს ციკლმა
            do
            {
                Console.Write("Enter User ID: ");
                //ვამოწმებთ UserID-ის ვალიდურობას და ვიწერთ მონაცემს userIDNumber-ში და გამოვდივართ ციკლიდან
                if (long.TryParse(Console.ReadLine(), out userIDNumber) && userIDNumber >= 0 && userIDNumber <= 99999999999)
                {
                    validUserID = true;
                }
                else
                {
                    Console.WriteLine("Invalid User ID. Please enter a valid 11-digit ID.");
                }
            } while (!validUserID);

            Console.Write("Enter Full Name of the User: ");
            userFullName = Console.ReadLine();

            string fileName = filePath + "\\User" + userIDNumber.ToString() + ".txt";
            //თუ ფაილი არ არსებობს ამ აიდით, შექმნის და ჩაწერს მოწოდებულ ინფორმაციას
            if (!File.Exists(fileName))
            {
                using (StreamWriter writer = File.CreateText(fileName))
                {
                    writer.WriteLine($"Name: {userFullName}");
                    writer.WriteLine("GEL: 0");
                    writer.WriteLine("USD: 0");
                    writer.WriteLine("EUR: 0");
                }
                Console.WriteLine("User added successfully.");
                Console.WriteLine("Add another User? (Y/N)");
                string repeatResponse;
                do
                {
                    repeatResponse = Console.ReadLine().ToUpper();
                    if (repeatResponse == "N")
                    {
                        Console.Clear();
                        MainMenu();
                        return;
                    }
                    else if (repeatResponse != "Y")
                    {
                        Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                    }
                } while (repeatResponse != "Y");
                AddUser();
            }
            //და თუ ფაილი არსებობს შეცდომა დაფიქსირდება და გვკითხავს ოპერაციის განმეორა გვსურს თუ არა.
            else
            {
                Console.WriteLine("User already exists.");
                Console.WriteLine("Add another User? (Y/N)");
                string repeatResponse;
                do
                {
                    repeatResponse = Console.ReadLine().ToUpper();
                    if (repeatResponse == "N")
                    {
                        Console.Clear();
                        MainMenu();
                        return;
                    }
                    else if (repeatResponse != "Y")
                    {
                        Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                    }
                } while (repeatResponse != "Y");
                AddUser();
            }
        }
        #endregion

        #region User Menu Functionalities
        //დეოპოზიტი
        public static void UpdateDepositBalance(long userID, string userFullName, string currency, int amount)
        {
            string fileName = filePath + "\\User" + userID.ToString() + ".txt";

            //ფაილში ბალანსის განახლება
            if (File.Exists(fileName))
            {
                //სტრინგში ჩავწეროთ ყველა line ტექსტური ფაილიდან, სადაც შემდეგ ვიპოვით სასურველ ვალუტას
                string[] lines = File.ReadAllLines(fileName);
                for (int i = 0; i < lines.Length; i++) //lines.Length ამ შემთხვევაში არის 4.
                {
                    if (lines[i].StartsWith(currency + ":")) //ვამოწმებთ ხაზი თუ იწყება ვალუტის დასახელებით + ":"
                    {
                        int currentBalance = int.Parse(lines[i].Substring(currency.Length + 2)); //აქ ჩავწერთ არსებულ ბალანსს ინტის სახით.  currency.Length + 2 -> ეს ნიშნავს რომ წინსართი, : და space გამოიტოვება. ანუ ავიღებთ მხოლოდ ინტეჯერს
                        lines[i] = currency + ": " + (currentBalance + amount); // და ამავე ხაზზე ისევ თავის ადგილას ჩავწერთ ახალ დამატებულ ბალანსს
                        break;
                    }
                }
                File.WriteAllLines(fileName, lines); // განახლებული ბალანსი ჩაიწერება კვლავ ფაილში
            }
            else
            {
                Console.WriteLine("User file not found. Unable to update balance.");
                EnterUserMenuUi(userID, userFullName);
            }
        }
        public static void UpdateWithdrawtBalance(long userID, string userFullName, string currency, int amount)
        {
            string fileName = filePath + "\\User" + userID.ToString() + ".txt";

            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(currency + ":"))
                    {
                        int currentBalance = int.Parse(lines[i].Substring(currency.Length + 2));
                        if (currentBalance - amount >= 0) //ყველაფერი იგივა რაც დეპოზიტისას გარდა იმისა, რომ აქ ვამოწმებთ გასატანი თანხა ხომ არ აღემატება არსებულ თანხას.
                        {
                            lines[i] = currency + ": " + (currentBalance - amount);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Insuficcient Balance on the account!");
                            EnterUserMenuUi(userID, userFullName);
                        }
                    }
                }
                File.WriteAllLines(fileName, lines);
            }
            else
            {
                Console.WriteLine("User file not found. Unable to update balance.");
                EnterUserMenuUi(userID, userFullName);
            }
        }
        public static void CheckBalance(long userID, string userFullName)
        {
            string fileName = filePath + "\\User" + userID.ToString() + ".txt";

            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);

                Console.WriteLine($"Current Balance for User {userFullName}:");
                foreach (string line in lines)
                {
                    if (line.StartsWith("GEL:") || line.StartsWith("USD:") || line.StartsWith("EUR:"))
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            else
            {
                Console.WriteLine("User file not found. Unable to update balance.");
                EnterUserMenuUi(userID, userFullName);
            }
        }
        #endregion

        #region MenuUi
        public static void UserMenuUi(long userID, string userFullName)
        //ყველა ოპერაციის შემდეგ ვკითხოთ მომხმარებელს დაასრულოს თუ არა მუშაობა. უარის შემთხვევაში დავბრუნდეთ მთავარ მენიუში.
        {
            Console.Clear();
            Console.WriteLine($"Hello {userFullName}!");
            Console.WriteLine("1. Deposit\t | 2. Withdraw\t | 3. Check Balance\t | 4. Exit");

            int choice;
            bool validChoice = false;

            do
            {
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            DepositMenuUi(userID, userFullName);
                            validChoice = false;
                            break;
                        case 2:
                            WithdrawMenuUi(userID, userFullName);
                            validChoice = false;
                            break;
                        case 3:
                            CheckBalanceMenuUi(userID, userFullName);
                            validChoice = false;
                            break;
                        case 4:
                            MainMenu();
                            validChoice = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                }
            }
            while (!validChoice);
        }
        public static void CheckBalanceMenuUi(long userID, string userFullName)
        {
            Console.Clear();
            Console.WriteLine($"Hello {userFullName}!");
            Console.WriteLine("1. Back to menu\t | 2. Exit");
            int choice;
            bool validChoice = false;

            CheckBalance(userID, userFullName);
            do
            {
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {

                    switch (choice)
                    {
                        case 1:
                            UserMenuUi(userID, userFullName);
                            validChoice = false;
                            break;
                        case 2:
                            MainMenu();
                            validChoice = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                }
            }
            while (!validChoice);
        }
        public static void DepositMenuUi(long userID, string userFullName)
        {
            Console.Clear();
            Console.WriteLine($"Hello User {userFullName}! Welcome to the deposit menu.");
            Console.WriteLine("Choose the currency you want to deposit:");
            Console.WriteLine("1. GEL (₾)\t | 2. USD ($)\t | 3. EUR (€)\t | 4. Exit to Menu");

            int choice;
            bool validChoice = false;
            //მენიუს ღილაკების ვალიდაცია
            do
            {
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 4)
                {
                    validChoice = true;
                    string currency = ""; // Initialize currency variable here
                    switch (choice)
                    {
                        case 1:
                            currency = "GEL";
                            break;
                        case 2:
                            currency = "USD";
                            break;
                        case 3:
                            currency = "EUR";
                            break;
                        case 4:
                            UserMenuUi(userID, userFullName); // Exit to the main menu
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                            break;
                    }
                    if (!string.IsNullOrEmpty(currency)) // Check if currency is not null or empty
                    {
                        Console.Write($"Enter the amount you want to deposit in {currency}: ");
                        if (int.TryParse(Console.ReadLine(), out int amount) && amount >= 0 && amount <= 5000)
                        {
                            UpdateDepositBalance(userID, userFullName, currency, amount);
                            Console.WriteLine($"Successfully deposited {amount} {currency}.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid amount. Please enter a valid ammount between 0 and 5000.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                }
            } while (!validChoice);

            //მომხმარებლის მენიუში დაბრუნება
            EnterUserMenuUi(userID, userFullName);
        }
        public static void WithdrawMenuUi(long userID, string userFullName)
        {
            Console.Clear();
            Console.WriteLine($"Hello User {userFullName}! Welcome to the Withdraw menu.");
            Console.WriteLine("Choose the currency you want to Withdraw:");
            Console.WriteLine("1. GEL (₾)\t | 2. USD ($)\t | 3. EUR (€)\t | 4. Exit to Menu");

            int choice;
            bool validChoice = false;

            do
            {
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 4)
                {
                    validChoice = true;
                    string currency = "";
                    switch (choice)
                    {
                        case 1:
                            currency = "GEL";
                            break;
                        case 2:
                            currency = "USD";
                            break;
                        case 3:
                            currency = "EUR";
                            break;
                        case 4:
                            UserMenuUi(userID, userFullName);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                            break;
                    }
                    if (!string.IsNullOrEmpty(currency))
                    {
                        Console.Write($"Enter the amount you want to Withdraw in {currency}: ");
                        if (int.TryParse(Console.ReadLine(), out int amount) && amount >= 0 && amount <= 5000)
                        {
                            UpdateWithdrawtBalance(userID, userFullName, currency, amount);
                            Console.WriteLine($"Successfully Withdrawn {amount} {currency}.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid amount. Please enter a valid ammount between 0 and 5000.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                }
            } while (!validChoice);

            UserMenuUi(userID, userFullName);
        }
        public static void EnterUserMenuUi(long userID, string userFullName) //სასურველი ოპერაციის შემდეგ ვკითხოთ მომხმარებელს სურს თუ არა სხვა ოპერაციის განხორციელება. უარის შემთხვევაში დავბრუნდეთ მთავარ მენიუში.
        {
            Console.WriteLine("\n___________________________________________");
            Console.WriteLine("Do you want Perform another Operation? (Y/N): ");
            string response = Console.ReadLine();
            if (response.ToUpper() == "Y")
            {
                Console.Clear();
                UserMenuUi(userID, userFullName);
            }
            else if (response.ToUpper() == "N")
            {
                Console.Clear();
                MainMenu();
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                FinishWorkPrompt();
            }
        }
        public static void FinishWorkPrompt() //ყველა ოპერაციის შემდეგ ვკითხოთ მომხმარებელს დაასრულოს თუ არა მუშაობა. უარის შემთხვევაში დავბრუნდეთ მთავარ მენიუში.
        {
            Console.WriteLine("\n___________________________________________");
            Console.WriteLine("Do you want to Exit? (Y/N): ");
            string response = Console.ReadLine();
            if (response.ToUpper() == "N")
            {
                Console.Clear();
                MainMenu();
            }
            else if (response.ToUpper() == "Y")
            {
                Console.Clear();
                Console.WriteLine("Exiting program.");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                FinishWorkPrompt();
            }
        }
        #endregion
    }
}