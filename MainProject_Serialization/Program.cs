using System;
using System.IO;
using System.Linq; // to be able to use count for input file
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary; //to use serialization


namespace MainProject_Serialization
{
    class Program
    {
        static void Main()
        {
            //MENU
            bool MenuFlag = true;
            //list of objects (customers)
            List<Customers> C = new List<Customers>();
            //Load Customers data from the file
            if (File.Exists("data.bin"))
            {
                C = ReadDataFromFile();
            }
            do
            {
                //display Menu
                int MainUserDecision = DisplayMainMenu();
                switch (MainUserDecision)
                {
                    case 1: //show list of customers
                        ShowListCustomers(C);
                        BackToMenu();
                        break;

                    case 2: //search a customer. Modify customer data
                        C = SearchCustomer(C);
                        break;

                    case 3: //create new customer
                        C = CreateNewCustomer(C);
                        break;

                    case 4: //show schedule
                        Schedule(C);
                        break;

                    case 5: //exit
                        Console.Clear();
                        WriteDataToFile(C); //write customers data into file
                        Console.WriteLine("SYSTEM CLOSED");
                        MenuFlag = false;
                        break;
                }
            } while (MenuFlag);
        }
        //FUNCTIONS
        static void Schedule(List<Customers> C)
        {
            Console.Clear();
            Console.WriteLine("     AGENT SCHEDULE");
            Console.WriteLine("(1) Search the visits scheduled for a certain month. \n(2) Search the next visit of a certain customer.");
            int UserDecision = readInt("Choose an option: ", 1, 2);
            switch (UserDecision)
            {
                case 1: //visits in a certain month
                    int yearOfMonth = readInt("Enter the year of the month: ", 2000, 3000);
                    int MonthToLook = readInt("Enter month to look (number): ", 1, 12);
                    //to convert month in number to string
                    string MonthToLookString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(MonthToLook);
                    Console.Write("\nVisits During " + MonthToLookString + " (" + yearOfMonth + ")");
                    Console.WriteLine("\nCUSTOMER       NEXT VISIT\n");
                    for (int i = 0; i < C.Count; i++)
                    {
                        if (C[i].getNextVisitYear() == yearOfMonth)
                        {
                            if (C[i].getNextVisitMonth() == MonthToLook)
                            {
                                Console.Write(C[i].getName());
                                Console.WriteLine("       " + C[i].getNextVisitDateString());
                            }
                        }
                    }
                    BackToMenu();
                    break;

                case 2: //next visit of a certain customer
                    string CustomerToLook = readString("Enter the Customer name to look: ");
                    for (int i = 0; i < C.Count; i++)
                    {
                        if (C[i].getName() == CustomerToLook)
                        {
                            Console.Write("\nThe next visit is: ");
                            Console.WriteLine(C[i].getNextVisitDateString());
                        }
                    }
                    BackToMenu();
                    break;
            }
        }
        static List<Customers> CreateNewCustomer(List<Customers> C)
        {
            Console.Clear();
            Console.WriteLine("CREATE A NEW CUSTOMER");
            bool NewCustomerFlag = true;
            do
            {
                //enter data
                string CustomerName = readString("Enter the Customer name: ");
                string CustomerCity = readString("Enter the Customer address (City): ");
                int CustomerLastVisitDay = readInt("Enter the Customer last visit (day): ", 1, 31);
                int CustomerLastVisitMonth = readInt("Enter the Customer last visit (month in number): ", 1, 12);
                int CustomerLastVisitYear = readInt("Enter the Customer last visit (year in number): ", 2000, 3000);
                int CustomerPeriodicity = readInt("Enter the periodicity of the visits(1 visit every 'x' months) per year: ", 1, 12);
                DateTime CustomerLastVisitDate = new DateTime(CustomerLastVisitYear, CustomerLastVisitMonth, CustomerLastVisitDay, new GregorianCalendar());
                //Add new customer (object) to the list
                C.Add(new Customers(CustomerName, CustomerCity, CustomerLastVisitDate, CustomerPeriodicity));
                //data created
                Console.WriteLine("New Customer created.");
                Console.Write("Name: ");
                Console.WriteLine(CustomerName);
                Console.Write("Address: ");
                Console.WriteLine(CustomerCity);
                Console.Write("Last Visit: ");
                Console.WriteLine(C.Last().getLastVisitDateString());
                Console.Write("Frequency of visits (every 'x' months): ");
                Console.WriteLine(CustomerPeriodicity);
                Console.Write("Next Visit: ");
                Console.WriteLine(C.Last().getNextVisitDateString());
                Console.WriteLine(" ");

                string UserDecision3 = readString("Would you like to add another customer? (Yes/No): ");
                if (UserDecision3 == "y" || UserDecision3 == "Y"|| UserDecision3 == "yes"|| UserDecision3 == "YES"|| UserDecision3 == "Yes")
                {
                    Console.WriteLine("NEW CUSTOMER:");
                }
                else
                {
                    BackToMenu();
                    NewCustomerFlag = false;
                }
            } while (NewCustomerFlag);
            return C;
        }
        static List<Customers> SearchCustomer(List<Customers> C)
        {
            Console.WriteLine("(1) Search by Customer Name.\n(2) Search by Customer Address.");
            int UserDecision1 = readInt("Select an option: ", 1, 2);
            switch (UserDecision1)
            {
                case 1: //search by customer name
                    string CustomerNameCase1 = readString("Enter the Customer name or a part of the name (Remember, the system differentiates between  upper & lower case letters): ");
                    Console.WriteLine("\n\nCUSTOMER(S):");
                    for (int i = 0; i < C.Count; i++)
                    {
                        if (C[i].getName().Contains(CustomerNameCase1))
                        {
                            Console.WriteLine(C[i].getName());
                        }
                    }
                    break;

                case 2: //search by customer address (location, city)
                    string CustomerLocation = readString("Enter the Customer location or a part of the location: ");
                    Console.WriteLine("\n\nCUSTOMER(S):");
                    for (int i = 0; i < C.Count; i++)
                    {
                        if (C[i].getAddress().Contains(CustomerLocation))
                        {
                            Console.WriteLine(C[i].getName());
                        }
                    }
                    break;
            }
            int UserDecision2 = readInt("\n\nWhat would you like to do know now?\n     (1) Remove one of the customers of the list. \n     (2) Modify data from one of the customers from the list.\n     (3) Back to Main Menu.\n", 1, 3);
            switch (UserDecision2)
            {
                case 1: //remove customer
                    string CustomerToRemove = readString("Enter the Customer name as it appears in the list: ");
                    for (int i = 0; i < C.Count; i++)
                    {
                        if (C[i].getName() == CustomerToRemove)
                        {
                            C.RemoveAt(i);
                            Console.WriteLine("CUSTOMER REMOVED.");
                        }
                    }
                    BackToMenu();
                    break;

                case 2: // modify customer
                    string CustomerToModify = readString("Enter the Customer name as it appears in the list: ");
                    for (int i = 0; i < C.Count; i++)
                    {
                        if (C[i].getName() == CustomerToModify)
                        {
                            int UserDecision3 = readInt("\nwhat would you like to modify?\n(1) Name.\n(2) Address.\n(3) Last visit date.\n(4) Periodicity.\n", 1, 4);
                            switch (UserDecision3)
                            {
                                case 1: //name
                                    string NewName = readString("\nEnter NEW name: ");
                                    C[i].setNewName(NewName);
                                    break;

                                case 2: //address
                                    string NewAddress = readString("\nEnter NEW address: ");
                                    C[i].setNewAddress(NewAddress);
                                    break;

                                case 3: //visit date
                                    Console.WriteLine("\nEnter NEW date:");
                                    int NewDateYear = readInt("    Year: ", 2000, 3000);
                                    int NewDateMonth = readInt("    Month(in number): ", 1, 12);
                                    int NewDateDay = readInt("    Day: ", 1, 31);
                                    C[i].setNewLastVisitDate(NewDateYear, NewDateMonth, NewDateDay);
                                    C[i].setNextVisitDate();
                                    break;

                                case 4: //periodicity
                                    int NewPeriodicity = readInt("\nNew Periodicity: ", 1, 12);
                                    C[i].setNewPeriodicity(NewPeriodicity);
                                    C[i].setNextVisitDate();
                                    break;
                            }
                            Console.WriteLine("CUSTOMER MODIFIED.");
                        }
                    }
                    BackToMenu();
                    break;
                case 3: //exit and back to main menu
                    break;
            }
            return C;
        }
        static void ShowListCustomers(List<Customers> C)
        {
            //display the table
            Console.WriteLine(" ");
            Console.Write("   NAME");
            Console.Write("       ADDRESS");
            Console.Write("        LAST VISIT");
            Console.Write("       VISIT PERIODICITY (every 'x' months)");
            Console.WriteLine("        NEXT VISIT");
            if (!C.Any()) //check if the list is empty
            {
                Console.WriteLine("      LIST OF CUSTOMERS EMPTY");
            }
            else
            {
                for (int i = 0; i < C.Count; i++)
                {
                    Customers CurrentCustomer = C[i];

                    Console.Write(" " + CurrentCustomer.getName());
                    Console.Write("       " + CurrentCustomer.getAddress());
                    Console.Write("       " + CurrentCustomer.getLastVisitDateString());
                    Console.Write("             " + CurrentCustomer.getPeriodicity());
                    Console.WriteLine("                                 " + CurrentCustomer.getNextVisitDateString());
                }
            }
        }
        static void BackToMenu()
        {
            Console.WriteLine("Press any key to return to the Main Menu.");
            Console.ReadKey();
        }
        static string readString(string prompt)
        {
            string result;
            do
            {
                Console.Write(prompt);
                result = Console.ReadLine();

            } while (result == "");
            return result;
        }
        static int readInt(string prompt, int low, int high)
        {
            int result;
            do
            {
                string intString = readString(prompt);
                try
                {
                    result = int.Parse(intString);
                }
                catch
                {
                    Console.WriteLine("Invalid!");
                    result = low - 1;
                }

            } while ((result < low) || (result > high));
            return result;
        }
        public static int DisplayMainMenu()
        {
            Console.Clear(); //to clean the console screen
            Console.WriteLine("         MENU");
            Console.WriteLine("");
            Console.WriteLine("1. Show a list of all the customers.");
            Console.WriteLine("2. Search for a customer or modify it data.");
            Console.WriteLine("3. Create a new customer.");
            Console.WriteLine("4. Show the agenda.");
            Console.WriteLine("5. Exit.");
            int MenuDecision = readInt("Choose an option: ", 1, 5);
            return MenuDecision;
        }
        public static void WriteDataToFile(List<Customers> C)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create("data.bin");
            bf.Serialize(file, C);
            file.Close();
        }
        public static List<Customers> ReadDataFromFile()
        {
            List<Customers> C = new List<Customers>();
            if (File.Exists("data.bin"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.OpenRead("data.bin");
                C = (List<Customers>)bf.Deserialize(file);
                file.Close();
            }
            else
            {
                throw new ArgumentException("The file does not exist.");
            }

            return C;
        }

    }
}

