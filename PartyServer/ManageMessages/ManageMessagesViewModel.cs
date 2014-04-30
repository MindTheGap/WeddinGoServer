using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PartyServer.DB;

namespace PartyServer.ManageMessages
{
    public class ManageMessagesViewModel
    {
        public ObservableCollection<Message> AllMessagesCollection { get; set; }

        public ManageMessagesViewModel(ObservableCollection<Message> AllMessagesCollection)
        {
            this.AllMessagesCollection = AllMessagesCollection;
        }
    }
}
