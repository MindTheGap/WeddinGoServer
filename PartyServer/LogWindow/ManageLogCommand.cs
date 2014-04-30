using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PartyServer.ViewModels;
using PartyServer.ManageUsers;
using PartyServer.DB;
using System.Collections.ObjectModel;

namespace PartyServer.LogWindow
{
	public class ManageLogCommand : ICommand
	{
		public MainWindowViewModel Context { get; set; }

        public ManageLogViewModel ManageLogViewModel { get; set; }

        public ManageLog ManageLogWindow { get; set; }

        public ManageLogCommand(MainWindowViewModel context)
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
            if (ManageLogWindow == null || ManageLogWindow.IsActive == false)
            {
                ManageLogWindow = new ManageLog();
                ManageLogViewModel = new ManageLogViewModel(new ObservableCollection<Log>(Context.dataEntities.Log));
                ManageLogWindow.DataContext = ManageLogViewModel;
            }
            
            ManageLogWindow.Show();
		}

		#endregion
	}
}
