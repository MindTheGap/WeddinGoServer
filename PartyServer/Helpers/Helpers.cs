using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartyServer.Helpers
{
    public class Helpers
    {
        public enum MessageTypeFromClient
        {
            Registration = 1,
            SearchResults,
            GetDetails,
            GetGreetings
        }

        public enum MessageTypeToClient
        {
            AOK = 1,
            Error,
        }

        public enum ErrorType
        {
            UserDoesNotExist,
            UserAlreadyExists,
            DetailsAreMissing
        }
    }
}
