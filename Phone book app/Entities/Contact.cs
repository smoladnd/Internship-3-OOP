using System;
using System.Collections.Generic;
using PhoneBookApp.Enums;

namespace PhoneBookApp.Entities
{
    public class Contact
    {
        public string NameAndSurname { get; set; }

        public string PhoneNumber { get; set; }

        public PreferenceType Preference { get; set; }

        public IList<Calls> Calls { get; set; } = new List<Calls>();

        public Contact AddContact(string nameAndSurname, string phoneNumber)
        {
            NameAndSurname = nameAndSurname;
            PhoneNumber = phoneNumber;
            Preference = PreferenceType.Normalan;
            return this;
        }

        public Contact EditContact(string nameAndSurname, string phoneNumber, int preference)
        {
            NameAndSurname = nameAndSurname;
            PhoneNumber = phoneNumber;
            Preference = (PreferenceType)preference;
            return this;
        }

        public override string ToString() => $"Kontakt: {NameAndSurname}\t Broj: {PhoneNumber}\t Status: {Preference}";
    }
}
