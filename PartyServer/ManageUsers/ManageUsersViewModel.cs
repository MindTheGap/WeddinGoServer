using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PartyServer.ViewModels;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using PartyServer.DB;

namespace PartyServer.ManageUsers
{
    public class ManageUsersViewModel : BaseViewModel
    {
        public ManageUsersViewModel(ObservableCollection<Member> AllUsersCollection, MainWindowViewModel MainWindowViewModel)
        {
            this.AllUsersCollection = AllUsersCollection;
            this.MainWindowViewModel = MainWindowViewModel;
        }

        public ObservableCollection<Member> AllUsersCollection { get; set; }

        public MainWindowViewModel MainWindowViewModel { get; set; }
    }
}
