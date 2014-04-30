using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PartyServer.ViewModels;
using PartyServer.ManageMessages;
using PartyServer.ManageUsers;
using System.Collections.ObjectModel;
using PartyServer.DB;

namespace PartyServer.CommandClasses
{
	public class ManageUsersCommand : ICommand
	{
		public MainWindowViewModel Context { get; set; }

        public ManageUsersViewModel ManageUsersViewModel { get; set; }

        public Views.ManageUsers ManageUsersWindow { get; set; }

        public ManageUsersCommand(MainWindowViewModel context)
		{
			this.Context = context;
		}

        
		#region ICommand Members

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter)
		{
            if (ManageUsersWindow == null || ManageUsersWindow.IsActive == false)
            {
                ManageUsersWindow = new Views.ManageUsers();
                ManageUsersViewModel = new ManageUsersViewModel(new ObservableCollection<Member>(this.Context.dataEntities.Member), Context);
                ManageUsersWindow.DataContext = ManageUsersViewModel;
            }
            
            ManageUsersWindow.Show();
		}

		#endregion
	}
}
