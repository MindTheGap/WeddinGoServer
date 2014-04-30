using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PartyServer.ViewModels;
using PartyServer.DB;
using System.Collections.ObjectModel;

namespace PartyServer.LogWindow
{
    public class ManageLogViewModel : BaseViewModel
    {
        public ObservableCollection<Log> AllLogCollection { get; set; }

        public ManageLogViewModel(ObservableCollection<Log> AllLogCollection)
        {
            this.AllLogCollection = AllLogCollection;
        }
    }
}
