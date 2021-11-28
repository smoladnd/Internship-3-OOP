using System;
using System.Collections.Generic;
using PhoneBookApp.Entities;
using PhoneBookApp.Enums;

namespace PhoneBookApp
{
    class Program
    {

        static void Main(string[] args)
        {
            do
            {
                var menuChoice = MainMenuInput();
                MainMenuChoice(menuChoice, ContactList);
            } while (PhoneBookIsActive);
        }

        public static int MainMenuInput()
        {
            var menuChoice = 0;
            bool correctInput;

            do
            {
                MainMenu();

                correctInput = int.TryParse(Console.ReadLine(), out menuChoice);

                if (correctInput)
                    break;

                Console.WriteLine("Vas odabir je nepostojan, molim vas pokusajte ponovno!\n");

            } while (!correctInput);

            return menuChoice;
        }

        private static void MainMenu()
        {
            if (ContactList.Keys.Count is not 0)
                Console.WriteLine
                    ("----------------------------------\n" +
                    "Main Contact Menu:\n" +
                    "1. - Ispis svih kontakata\n" +
                    "2. - Dodavanje novih kontakata u imenik\n" +
                    "3. - Brisanje kontakata iz imenika\n" +
                    "4. - Editiranje preference kontakata\n" +
                    "5. - Upravljanje kontaktom\n" +
                    "6. - Ispis svih poziva\n" +
                    "7. - Izlaz iz aplikacije\n" +
                    "-----------------------------------");
            else
                Console.WriteLine
                    ("--------------------------------------------------------------------\n" +
                    "Trenutno nema nikakvih kontakata pa imate samo ove dvije opcije:\n" +
                    "1 - Dodavanje novog kontakta\n" +
                    "2 - Izlaz iz aplikacije\n" +
                    "---------------------------------------------------------------------");
        }

        private static void MainMenuChoice(int menuChoice, IDictionary<Contact, List<Calls>> ContactList)
        {
            if (ContactList.Keys.Count is not 0)
            {
                switch (menuChoice)
                {
                    case 1:
                        WriteOutContacts(ContactList);
                        break;
                    case 2:
                        AddNewContact(ContactList);
                        break;
                    case 3:
                        EraseContact(ContactList);
                        break;
                    case 4:
                        EditingContactPreference(ContactList);
                        break;
                    case 5:
                        PhoneBookSubmenu(ContactList);
                        break;
                    case 6:
                        WriteOutAllCalls(ContactList);
                        break;
                    case 7:
                        PhoneBookIsActive = !PhoneBookIsActive;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Molim vas upisite jedan od ponudenih sedam brojeva!\n");
                        break;
                }
            }
            else
            {
                switch (menuChoice)
                {
                    case 1:
                        AddNewContact(ContactList);
                        break;
                    case 2:
                        PhoneBookIsActive = !PhoneBookIsActive;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Molim vas upisite jedan od dva moguca broja!\n");
                        break;
                }
            }

        }

    }
}
