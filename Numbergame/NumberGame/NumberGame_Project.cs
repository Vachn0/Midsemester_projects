using System;

namespace NumberGame
{
    internal class NumberGame
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            GameStart();
        }

        private static void GameStart()
        {
            bool askChangeDifficulty = true; //საჭიროა თამაშის დასარესტარტებლად
            while (askChangeDifficulty)
            {
                Console.WriteLine("Choose Game mode: 1 - Easy; 2 - Medium; 3 - Hard");

                if (!int.TryParse(Console.ReadLine(), out int gameMode) || gameMode < 1 || gameMode > 3)  //ამოწმებს input-ს თუ ის სცდება ფარგლებს
                {
                    Console.WriteLine("Invalid input! Please choose the game mode!");
                    Console.Clear();
                    continue;
                }
                Console.WriteLine("______________________________________________________");

                switch (gameMode) //სირთულის მიხედვით ცვლის ცდების რაოდენობას
                {
                    //პირველი მონაცემი არის კომპიუტერის მინიმალული ციფრი, მეორე მონაცემი არის მაქსიმალური და მესამე მონაცემი არის ცდების რაოდენობა
                    case 1:
                        GuessMyNumber(0, 100, 10);
                        break;
                    case 2:
                        GuessMyNumber(0, 100, 5);
                        break;
                    case 3:
                        GuessMyNumber(0, 100, 3);
                        break;
                    default:
                        Console.WriteLine("Invalid game mode selected.");
                        break;
                }

                askChangeDifficulty = restartGame(); //თამაშის წარმატებით დასრულების შემდეგ ეკრანზე გამოვა დარესტარტების მოთხოვნა
            }
        }

        static void GuessMyNumber(int minRange, int maxRange, int maxTries)  //ფუნქცია იღებს მინიმალურ და მაქსიმალურ ზღვარს რომ აიღოს რანდომ რიცხვი, და ასევე ცდების რაოდენობას.
        {
            int guess = random.Next(minRange, maxRange + 1);

            Console.WriteLine($"Guess my number! You have {maxTries} Tries.");
            Console.WriteLine("______________________________________________________");
            int tries = maxTries;  //საჭიროა რომ ყოველ "ვერ გამოცნობაზე" დააკლოს tries მაგრამ maxTries გამოიტანოს უცვლელი
            while (tries > 0)
            {
                Console.WriteLine("______________________________________________________");
                Console.Write("What is my Number?: ");
                if (!int.TryParse(Console.ReadLine(), out int userInput))
                {
                    Console.WriteLine("Invalid input! Please enter a valid number.");
                    continue;
                }
                Console.WriteLine("______________________________________________________");

                if (userInput < guess)  //თუ შეყვანილი რიცხვი დაბალია გამოაქვს შეტყობინება რომ შეიყვანოს უფრო მაღალი და დააკლოს მცდელობის რაოდენობა
                {
                    Console.WriteLine("Higher");
                    tries--;
                }
                else if (userInput > guess)
                {
                    Console.WriteLine("Lower"); //აქ პირიქით, როცა მაღალია გვეუბნება რომ შევიყვანოთ დაბალი და დააკლოს მცედლობის რაოდენობა
                    tries--;
                }
                else
                {
                    Console.WriteLine($"Congratulations! You guessed it in {maxTries - tries + 1} tries!");  //გამოცნობის შემთხვევაში გვეუბნება რამდენ ცდაში გამოვიცანით
                    return;
                }

                Console.WriteLine($"You have {tries} tries left!");
                Console.WriteLine("______________________________________________________");
            }

            Console.WriteLine("______________________________________________________");
            Console.WriteLine($"\nSorry, the number was {guess}. Better luck next time!");  //თამაშის წაგების შემთხვევაში გამოაქვს შეტყობინება თუ რა ციფრი ჰქონდა ჩაფიქრებული
            Console.WriteLine("______________________________________________________");
        }

        private static bool restartGame()  //თამაშის რესტარტ ფუნქცია
        {
            Console.WriteLine("______________________________________________________");
            Console.Write("Do you want to play again? (y/n): ");
            string response = Console.ReadLine().ToLower();
            Console.Clear();
            if (response != "y")
            {
                Console.Clear();
                Console.WriteLine("Good Bye!");
                return false;  //თუ არ არის y თამაში მთავრდება
            }
            return true;  // თუ y არის თამაში რესტარტდება
        }
    }
}