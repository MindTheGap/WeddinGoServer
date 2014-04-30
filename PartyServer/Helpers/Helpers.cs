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
            SearchResults,
            GetAllJoinedWeddings,
            LikeGreeting,
            Registration
        }

        public enum MessageTypeToClient
        {
            SearchResults,
            GetAllJoinedWeddings,
            Registration,
            AOK,
            Error
        }

        public enum ErrorType
        {
            UserDoesNotExist,
            UserAlreadyExists,
            DetailsAreMissing
        }
    }
}
