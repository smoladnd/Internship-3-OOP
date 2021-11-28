using System;
using System.Collections.Generic;
using PhoneBookApp.Entities;
using PhoneBookApp.Enums;

namespace PhoneBookApp
{
    class Program
    {
        public static Random NumberGenerator = new Random();
        public static int RandomCallConection => NumberGenerator.Next(2);
        public static int RandomCallDuration => NumberGenerator.Next(1, 20);


        public static bool PhoneBookIsActive = true;
        public static IDictionary<Contact, List<Calls>> ContactList = new Dictionary<Contact, List<Calls>>();

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

        private static void WriteOutContacts(IDictionary<Contact, List<Calls>> ContactList)
        {
            foreach (var item in ContactList)
            {
                Console.WriteLine(item.Key.ToString() +
                    "\n");
            }
        }

        private static void AddNewContact(IDictionary<Contact, List<Calls>> ContactList)
        {
            var check = false;
            string phoneNumber, nameAndSurname;
            var numberOfTries = 0;

            Console.Clear();
            Console.WriteLine("Upisite ime i prezime novog kontakta.");

            do
            {
                nameAndSurname = Console.ReadLine();
                check = CheckNameAndSurname(nameAndSurname);
            } while (check is false);

            Console.WriteLine("Upisite broj mobitela novog kontakta.");

            do
            {
                phoneNumber = Console.ReadLine();

                check = CheckPhoneNumber(phoneNumber, ContactList);
                numberOfTries++;

                if (numberOfTries is 4)
                {
                    Console.WriteLine("Vidim da se mucite sa upisom mobilnog broja, ako se zelite vratiti na glavni izbornik napisite 'da'.");
                    var choice = Console.ReadLine();

                    if (choice is "da" || choice is "DA")
                        break;
                    else
                        numberOfTries = 0;
                }

            } while (check is false);

            ContactList.Add(AddNewContact(nameAndSurname, phoneNumber), new List<Calls>());

            Console.WriteLine("Kontakt uspjesno upisan!\n" +
                "Ako zelite dodati jos neki kontakt napisite 'da', ako ne zelite stisnite bilo koji botun.");
            var addContactChoice = Console.ReadLine();

            if (addContactChoice is "DA" || addContactChoice is "da")
                AddNewContact(ContactList);
        }

        private static void EraseContact(IDictionary<Contact, List<Calls>> ContactList)
        {
            bool erasureConfirmed = true, isNumber;
            var chosenPhoneNumber = "";

            Console.Clear();

            WriteOutContacts(ContactList);
            Console.WriteLine("\nIspisite broj osobe od ponudenih koji zelite obrisati:");

            do
            {
                chosenPhoneNumber = Console.ReadLine();
                isNumber = int.TryParse(chosenPhoneNumber, out _);

                if (!isNumber)
                    Console.WriteLine("Mobilni broj vam se mora samo sastojati od brojeva! Pokusajte ponovno.");
                else
                    break;
            } while (true);

            foreach (var item in ContactList)
            {
                if (item.Key.PhoneNumber == chosenPhoneNumber)
                {
                    Console.WriteLine("Napisite 'da' ako ste sigurni da zelite izbrisati kontakt\n\n" + item.Key.ToString());
                    var erasionChoice = Console.ReadLine();

                    if (erasionChoice is "da" || erasionChoice is "DA")
                    {
                        if (item.Value is not null)
                            item.Value.Clear();

                        ContactList.Remove(item.Key);

                        Console.WriteLine("Kontakt je uspjesno izbrisan!");
                        erasureConfirmed = !erasureConfirmed;
                    }
                }
            }

            if (erasureConfirmed)
                Console.WriteLine("\nNaveden broj nije u listi kontakata");

            if (ContactList.Count > 0)
            {
                Console.WriteLine("\nAko zelite pokusat izbrisat neki drugi kontakt napisite 'da'.");
                var erasureRepeatChoice = Console.ReadLine();

                if (erasureRepeatChoice is "da" || erasureRepeatChoice is "DA")
                    EraseContact(ContactList);
            }
            else
                Console.WriteLine("Nema vise mogucih kontakata za brisanje, vraceni ste na main menu.\n");
        }

        private static void EditingContactPreference(IDictionary<Contact, List<Calls>> ContactList)
        {
            var chosenPhoneNumber = "";
            bool isNumber, successfulPreferenceEdit = false;

            Console.Clear();

            WriteOutContacts(ContactList);
            Console.WriteLine("\nIspisite broj osobe od ponudenih kojoj zelite promjenit preferencu:");

            do
            {
                chosenPhoneNumber = Console.ReadLine();
                isNumber = int.TryParse(chosenPhoneNumber, out _);

                if (!isNumber)
                    Console.WriteLine("Mobilni broj vam se mora samo sastojati od brojeva! Pokusajte ponovno.");
                else
                    break;
            } while (true);

            foreach (var item in ContactList)
            {
                if (item.Key.PhoneNumber == chosenPhoneNumber)
                {
                    Console.WriteLine("Odaberite jedno od ponudenih opcija:\n" +
                                      "0 - Favorit\n" +
                                      "1 - Normalan\n" +
                                      "2 - Blokiran");

                    var preferenceChoice = int.Parse(Console.ReadLine());
                    var newPreference = (Enums.PreferenceType)preferenceChoice;

                    if (item.Key.Preference == newPreference)
                    {
                        Console.WriteLine("Odabrana preferenca je vec na snazi u odabranom kontaktu!\n");
                        return;
                    }

                    Console.WriteLine("Napisite 'da' ako ste sigurni da zelite promjeniti preference kontakta\n\n" + item.Key.ToString());
                    var editChoice = Console.ReadLine();

                    if (editChoice is "da" || editChoice is "DA")
                    {
                        ContactList.Add(ChangingPrefrence(item.Key.NameAndSurname, item.Key.PhoneNumber, preferenceChoice), item.Value);

                        if (item.Value is not null)
                            item.Value.Clear();

                        ContactList.Remove(item.Key);

                        successfulPreferenceEdit = true;
                        break;
                    }
                    else
                        return;
                }
            }

            if (successfulPreferenceEdit is false)
            {
                Console.WriteLine("Odabran broj nije na listi kontakata!\n" +
                                  "Ako zelite pokusat ponovno napisite 'da'.");
                var repeatEdit = Console.ReadLine();

                if (repeatEdit is "da" || repeatEdit is "DA")
                    EditingContactPreference(ContactList);
            }

            if (successfulPreferenceEdit is true)
            {
                Console.WriteLine("Preferenca kontakta uspjesno promjenjena!\n" +
                              "Ako zelite promjeniti preferencu jos nekog kontakta napisite 'da', ako ne zelite stisnite bilo koji botun.");
                var editContactChoice = Console.ReadLine();

                if (editContactChoice is "DA" || editContactChoice is "da")
                    EditingContactPreference(ContactList);
            }
        }

        private static void PhoneBookSubmenu(IDictionary<Contact, List<Calls>> ContactList)
        {
            var repeat = false;

            Console.WriteLine("Odaberite jednu od sljedecih akcija:\n" +
                              "1 - Ispis svih poziva odabranog kontakta\n" +
                              "2 - Kreirajte novi poziv\n" +
                              "3 - Izlaz iz podmenua");

            var choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    WriteOutCallsOfContactSorted(ContactList);
                    break;
                case 2:
                    MakeNewCall(ContactList);
                    break;
                case 3:
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Molim vas upisite jedan od ponudenih tri broja!\n");
                    repeat = true;
                    break;
            }

            if (repeat is true)
                PhoneBookSubmenu(ContactList);
        }

        public static void WriteOutAllCalls(IDictionary<Contact, List<Calls>> ContactList)
        {
            Console.Clear();
            var counter = 0;

            if (ContactList.Values.Count is not 0)
            {
                var check = CheckForActiveCalls(ContactList);

                foreach (var item in ContactList)
                    if (item.Value is not null)
                        foreach (var thing in item.Value)
                        {
                            Console.WriteLine(thing.ToString());
                            counter++;
                        }
            }
            else
                Console.WriteLine("Nema zapisanih poziva trenutno!\n");
        }

        static Contact AddNewContact(string nameAndSurname, string phoneNumber)
        {
            var newContact = new Contact();
            newContact.AddContact(nameAndSurname, phoneNumber);
            return newContact;
        }

        public static Contact ChangingPrefrence(string nameAndSurname, string phoneNumber, int newPreference)
        {
            var editedContact = new Contact();
            editedContact.EditContact(nameAndSurname, phoneNumber, newPreference);
            return editedContact;
        }

        static Calls AddNewCall(DateTime callStart, CallStatus status, int duration)
        {
            var newCall = new Calls();
            newCall.AddNewCallInDictionary(callStart, status, duration);
            return newCall;
        }

        private static bool CheckNameAndSurname(string nameAndSurname)
        {
            var check = true;
            var countSpace = 0;

            for (int i = 0; i < nameAndSurname.Length; i++)
            {
                if (nameAndSurname[i] >= 'a' && nameAndSurname[i] <= 'z' || nameAndSurname[i] == ' ' || nameAndSurname[i] >= 'A' && nameAndSurname[i] <= 'Z')
                    check = true;
                else
                {
                    check = false;
                    Console.WriteLine("Molim vas pri upisu imena i prezime koristitie samo latinska slova!");
                    break;
                }
            }

            if (check is true)
            {
                for (int i = 0; i < nameAndSurname.Length; i++)
                    if (nameAndSurname[i] == ' ')
                        countSpace++;

                if (countSpace is 0)
                {
                    check = false;
                    Console.WriteLine("Molim vas kada pisete ime i prezime, pazite da stavite razmak izmedu svakog imena i prezimena.");
                }
            }

            return check;
        }

        private static bool CheckPhoneNumber(string phoneNumber, IDictionary<Contact, List<Calls>> ContactList)
        {
            var check = true;

            var isNumber = int.TryParse(phoneNumber, out _);
            if (!isNumber && check)
            {
                Console.WriteLine("Mobilni broj vam se mora samo sastojati od brojeva! Pokusajte ponovno.");
                return !check;
            }

            foreach (var item in ContactList)
            {
                if (item.Key.PhoneNumber == phoneNumber)
                {
                    Console.WriteLine("Mobilni broj vec postoji u listi kontakata! Pokusajte ponovno.");
                    return !check;
                }
            }

            return check;
        }

        public static void WriteOutCallsOfContactSorted(IDictionary<Contact, List<Calls>> ContactList)
        {
            var chosenPhoneNumber = "";
            bool isNumber = false;
            Console.Clear();

            var check = CheckForActiveCalls(ContactList);

            WriteOutContacts(ContactList);
            Console.WriteLine("\nIspisite broj kontakta od ponudenih kojem zelite ispisat pozive:");

            do
            {
                chosenPhoneNumber = Console.ReadLine();
                isNumber = int.TryParse(chosenPhoneNumber, out _);

                if (!isNumber)
                    Console.WriteLine("Mobilni broj vam se mora samo sastojati od brojeva! Pokusajte ponovno.");
                else
                    break;
            } while (true);

            foreach (var item in ContactList)
                if (item.Key.PhoneNumber == chosenPhoneNumber)
                {
                    if (item.Value.Count is 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Nema zapisanih poziva na odabranom kontaktu!\n");
                        PhoneBookSubmenu(ContactList);
                    }
                    else
                    {
                        var temporariCallsList = new List<Calls>();
                        var tempCallDates = new List<DateTime>();

                        foreach (var thing in item.Value)
                        {
                            temporariCallsList.Add(AddNewCall(thing.BeginningOfCall, thing.StatusOfCall, thing.CallDuration));
                            tempCallDates.Add(thing.BeginningOfCall);
                        }

                        SortListDescending(tempCallDates);

                        foreach (var firstItem in tempCallDates)
                            foreach (var secondItem in temporariCallsList)
                                if (firstItem == secondItem.BeginningOfCall)
                                    Console.WriteLine(secondItem.ToString());
                    }
                }

        }

        public static void MakeNewCall(IDictionary<Contact, List<Calls>> ContactList)
        {
            var chosenPhoneNumber = "";
            bool isNumber, check, madeCall = false;

            Console.Clear();

            check = CheckForActiveCalls(ContactList);

            if (check is false)
            {
                WriteOutContacts(ContactList);
                Console.WriteLine("\nIspisite broj osobe od ponudenih koju zelite nazvati:");

                do
                {
                    chosenPhoneNumber = Console.ReadLine();
                    isNumber = int.TryParse(chosenPhoneNumber, out _);

                    if (!isNumber)
                        Console.WriteLine("Mobilni broj vam se mora samo sastojati od brojeva! Pokusajte ponovno.");
                    else
                        break;
                } while (true);

                foreach (var item in ContactList)
                {
                    if (item.Key.PhoneNumber == chosenPhoneNumber)
                    {
                        if (item.Key.Preference == PreferenceType.Blokiran)
                        {
                            Console.WriteLine("Odabrani broj je blokiran, vracam vas na pod menu!");
                            return;
                        }
                        else
                        {
                            var callChoice = "";
                            switch ((CallStatus)RandomCallConection)
                            {
                                case CallStatus.Propusten:
                                    var call = AddNewCall(DateTime.Now, CallStatus.Propusten, 0);
                                    ContactList[item.Key].Add(call);
                                    madeCall = !madeCall;
                                    Console.WriteLine("Poziv je propusten, zelite li natrag u podmenu?\n");
                                    callChoice = Console.ReadLine();

                                    if (callChoice is "da" || callChoice is "Da")
                                        MakeNewCall(ContactList);
                                    break;
                                case CallStatus.Traje:
                                    item.Value.Add(AddNewCall(DateTime.Now, CallStatus.Traje, RandomCallDuration));
                                    madeCall = !madeCall;
                                    Console.WriteLine("Poziv je u tijeku, zelite li natrag u podmenu?\n");
                                    callChoice = Console.ReadLine();

                                    if (callChoice is "da" || callChoice is "Da")
                                        MakeNewCall(ContactList);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (madeCall is false)
                {
                    Console.WriteLine("Odabran broj nije na listi kontakata, ako zelite pokusat ponovno napisite 'da'.");
                    var repeatChoice = Console.ReadLine();

                    if (repeatChoice is "da" || repeatChoice is "Da")
                        MakeNewCall(ContactList);
                }
            }
            else
            {
                Console.WriteLine("Trenutno ste u pozivu, sacekajte da zavrsi!\n");
                PhoneBookSubmenu(ContactList);
            }
        }

        public static bool CheckForActiveCalls(IDictionary<Contact, List<Calls>> ContactList)
        {
            bool check = false;

            foreach (var item in ContactList)
                if (item.Value is not null)
                    foreach (var thing in item.Value)
                        if (thing.StatusOfCall == CallStatus.Traje)
                        {
                            if (thing.BeginningOfCall.AddSeconds(thing.CallDuration) < DateTime.Now)
                                thing.StatusOfCall = CallStatus.Zavrsen;
                            else
                                return check = true;
                        }

            return check;
        }

        public static void SortListDescending(List<DateTime> temporariCallDateList)
        {
            var throwawayList = new List<DateTime>();
            var min = new DateTime(1, 1, 1, 0, 0, 0);

            while (temporariCallDateList.Count is not 0)
            {
                foreach (var item in temporariCallDateList)
                    if (item < min)
                    {
                        min = item;
                        break;
                    }
                throwawayList.Add(min);
                temporariCallDateList.Remove(min);
            }

            foreach (var item in throwawayList)
                temporariCallDateList.Add(item);
        }
    }
}

