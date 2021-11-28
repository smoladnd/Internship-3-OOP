using System;
using System.Collections.Generic;
using PhoneBookApp.Enums;

namespace PhoneBookApp.Entities
{
    public class Calls
    {
        public DateTime BeginningOfCall { get; set; }

        public CallStatus StatusOfCall { get; set; }

        public int CallDuration { get; set; }


        public void AddNewCallInDictionary (DateTime callStart, CallStatus status, int duration)
        {
            BeginningOfCall = callStart;
            StatusOfCall = status;
            CallDuration = duration;
        }

        public override string ToString() => $"Vrijeme poziva: {BeginningOfCall}\t Status poziva: {StatusOfCall}";
        
    }
}
