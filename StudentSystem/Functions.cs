using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem
{
    internal class Functions
    {
        // ახალი სტუდენტის შექმნა
        public static void AddStudent()
        {
            int rollId;
            string studentName;
            int studentGrade;
            bool validRollID = false;

            do  //სანამ ვალიდური არ იქნება rollId მანამ იმუშაოს ციკლმა
            {
                Console.Write("Enter RollID of the student: ");
                if (int.TryParse(Console.ReadLine(), out rollId))  //ვამოწმებთ RollID-ის ვალიდურობას და ვიწერთ მონაცემს rollId-ში და გამოვდივართ ციკლიდან
                {
                    validRollID = true;
                }
                else
                {
                    Console.WriteLine("Invalid RollID. Please enter a valid Student ID.");
                }
            }
            while (!validRollID);

            Console.Write("Enter Full-Name of the student: ");
            studentName = Console.ReadLine();

            do //სანამ break არ შესრულდება ციკლი ჩართული იქნება
            {
                Console.Write("Enter grade of the student (between 0 and 100): ");
                if (int.TryParse(Console.ReadLine(), out studentGrade))  //იღებს მხოლოდ იმ studentGrade-ს რომელიც 0-100 შუალედში იქნება
                {
                    if (studentGrade < 0 || studentGrade > 100)
                    {
                        Console.WriteLine("Invalid grade entered. Grade must be between 0 and 100.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid grade entered. Please enter a valid Grade 0-100.");
                }
            }
            while (true);

            if (Student.StudentsList.Any(s => s.RollID == rollId))  //თუ ამ აიდით არსებობს სტუდენტი გამოიტანოს შეტყობინება, წინააღმდეგ შემთხვევაში გადავა else სეგმენტზე
            {
                Console.WriteLine($"Error: Student with RollID {rollId} already exists.");
                ReturnToMainMenuOrExit();
            }
            else
            {
                Student newStudent = new Student
                {
                    RollID = rollId,
                    StudentName = studentName,
                    StudentGrade = studentGrade
                };

                Student.StudentsList.Add(newStudent);
                Console.WriteLine("___________________________________________");
                Console.WriteLine($"Student added: {newStudent.RollID} | {newStudent.StudentName} | {newStudent.StudentGrade}");
                Console.WriteLine("___________________________________________");
                ReturnToMainMenuOrExit();  //წარმატებით დასრულების შემდეგ გვკითხოს მენიუში გვინდა გადასვლა თუ პროგრამიდან გამოსვლა
            }
        }

        // სტუდენტის წაშლა
        public static void RemoveStudent()
        {
            int rollIdDelete;
            bool validRollID = false;

            do
            {
                Console.Write("Enter the RollID of the student you want to remove: ");
                if (int.TryParse(Console.ReadLine(), out rollIdDelete))  //მიიღებს მხოლოდ ვალიდურ rollID-ს და პროგრამა გამოვა ციკლიდან
                {
                    validRollID = true;
                }
                else
                {
                    Console.WriteLine("Invalid RollID. Please enter a valid integer.");
                }
            }
            while (!validRollID);

            Student studentToRemove = Student.StudentsList.FirstOrDefault(s => s.RollID == rollIdDelete);  //თუ შეყვანილი rollIdDelete ემთხვევა სიაში არსებულ RollID-ს მაშინ studentToRemove-ში ჩაიწერება ის კონკრეტული ობიექტი.
            if (studentToRemove != null)  //სტუდენტის პოვნის შემთხვევაში წავშალოთ სიიდან
            {
                Student.StudentsList.Remove(studentToRemove);
                Console.WriteLine($"Student with RollID {rollIdDelete} removed successfully.");
                ReturnToMainMenuOrExit();
            }
            else
            {
                Console.WriteLine("___________________________________________");
                Console.WriteLine($"Student with RollID {rollIdDelete} not found.");
                Console.WriteLine("___________________________________________");
                ReturnToMainMenuOrExit();
            }
        }

        // ყველა სტუდენტის გამოტანა
        public static void ShowAllStudents()
        {
            foreach (var student in Student.StudentsList)
            {
                Console.WriteLine($"Student ID: {student.RollID}\t Student Name: {student.StudentName}\t Grade: {student.StudentGrade}");
            }
            ReturnToMainMenuOrExit();
        }

        // სტუდენტის ძიება
        public static void SearchStudentById()
        {
            int rollIDSearch;
            bool validRollID = false;

            do
            {
                Console.Write("Enter the RollID of the student you want to search for: ");
                if (int.TryParse(Console.ReadLine(), out rollIDSearch)) //ვალიდური ინპუტის შემთხვევაში ჩაიწერება მონაცემი rollIDSearch-ში და გამოვალთ ციკლიდან
                {
                    validRollID = true;
                }
                else
                {
                    Console.WriteLine("Invalid RollID. Please enter a valid Student ID.");
                }
            }
            while (!validRollID);

            Student studentFound = Student.StudentsList.FirstOrDefault(s => s.RollID == rollIDSearch); //თუ ჩაწერილი აიდი არსებობს სიაში studentFound-ში ჩავწეროთ ეს ობიექტი და გამოვიტანოთ
            if (studentFound != null)
            {
                Console.WriteLine($"Student found with RollID {rollIDSearch}:");
                Console.WriteLine($"Student ID: {studentFound.RollID}\t Student Name: {studentFound.StudentName}\t Grade: {studentFound.StudentGrade}");
                ReturnToMainMenuOrExit();
            }
            else
            {
                Console.WriteLine("___________________________________________");
                Console.WriteLine($"Student with RollID {rollIDSearch} not found.");
                Console.WriteLine("___________________________________________");
                ReturnToMainMenuOrExit();
            }
        }

        // სტუდენტის ქულის შეცვლა
        public static void EditStudentGrade()
        {
            int rollIDToEdit;
            bool validRollID = false;

            do
            {
                Console.Write("Enter the RollID of the student whose grade you want to edit: ");
                if (int.TryParse(Console.ReadLine(), out rollIDToEdit)) //ვალიდური აიდის მიღების შემთხვევაში ჩაიწერება rollIDToEdit-ში და გამოვალთ ციკლიდან
                {
                    validRollID = true;
                }
                else
                {
                    Console.WriteLine("Invalid RollID. Please enter a valid Student Roll ID!");
                }
            }
            while (!validRollID);

            Student studentToEdit = Student.StudentsList.FirstOrDefault(s => s.RollID == rollIDToEdit);  // იპოვის ამ აიდით სიაში ობიექტს და ჩაწერს studenToEdit-ში
            if (studentToEdit != null)
            {
                Console.WriteLine($"Student found with RollID {rollIDToEdit}:");
                Console.WriteLine($"Student Name: {studentToEdit.StudentName}\nCurrent Grade: {studentToEdit.StudentGrade}");

                int newGrade;

                // სანამ ვალიდურ ქულას არ ჩავწერთ ციკლი არ გაითიშება. ვალიდური ქულა კი იწერება newGrade-ში
                do
                {
                    Console.Write("Enter the new grade (between 0 and 100): ");
                    if (int.TryParse(Console.ReadLine(), out newGrade))
                    {
                        if (newGrade < 0 || newGrade > 100)
                        {
                            Console.WriteLine("Invalid grade entered. Grade must be between 0 and 100.");
                        }
                        else
                        {
                            studentToEdit.StudentGrade = newGrade;
                            Console.WriteLine("Grade updated successfully.");
                            ReturnToMainMenuOrExit();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid grade entered. Please enter a valid Grade.");
                    }
                } while (newGrade < 0 || newGrade > 100);
            }
            else
            {
                Console.WriteLine("___________________________________________");
                Console.WriteLine($"Student with RollID {rollIDToEdit} not found.");
                Console.WriteLine("___________________________________________");
                EditStudentGrade();
            }
        }
        // მთავარი მენიუ (მხოლოდ ეს გავიტანეთ main-ში)
        public static void MainMenu()
        {
            Console.WriteLine("Welcome to student Grading System!");
            Console.WriteLine("1. Add Student\t | 2. Delete Student\t | 3. Search Student\t | 4. Display All Students\t | 5. Edit Grade\t | 6. Exit");

            int choice;
            bool validChoice = false;

            do //იღებს მხოლოდ ინტეჯერს.
            {
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice) //თითო ღილაკით გამოვიძახოთ სასურველი ფუნქცია
                    {
                        case 1:
                            AddStudent();
                            validChoice = false;
                            break;
                        case 2:
                            RemoveStudent();
                            validChoice = false;
                            break;
                        case 3:
                            SearchStudentById();
                            break;
                        case 4:
                            ShowAllStudents();
                            validChoice = false;
                            break;
                        case 5:
                            EditStudentGrade();
                            validChoice = false;
                            break;
                        case 6:
                            Console.Clear();
                            Console.WriteLine("Exiting program.");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a valid integer.");
                }
            }
            while (!validChoice);

            
        }
        public static void ReturnToMainMenuOrExit() //ყველა ოპერაციის შემდეგ ვკითხოთ მომხმარებელს გადავიდეს თუ არა მთავარ მენიუში. უარის შემთხვევაში დავასრულოთ მუშობა.
        {
            Console.WriteLine("\n___________________________________________");
            Console.WriteLine("Do you want to return to the main menu? (Y/N): ");
            string response = Console.ReadLine();
            if (response.ToUpper() == "Y")
            {
                Console.Clear();
                MainMenu();
            }
            else if (response.ToUpper() == "N")
            {
                Console.WriteLine("Exiting program.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                ReturnToMainMenuOrExit();
            }
        }
    }
}
