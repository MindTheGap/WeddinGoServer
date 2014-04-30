using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PartyServer.ViewModels;
using System.Collections.ObjectModel;
using PartyServer.DB;

namespace PartyServer.ManageMessages
{
	public class ManageMessagesCommand : ICommand
	{
		public MainWindowViewModel Context { get; set; }

        public ManageMessagesViewModel ManageMessagesViewModel { get; set; }

        public ManageMessages ManageMessagesWindow { get; set; }

		public ManageMessagesCommand(MainWindowViewModel context)
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
            if (ManageMessagesWindow == null || ManageMessagesWindow.IsActive == false)
            {
                ManageMessagesWindow = new ManageMessages();
                this.ManageMessagesViewModel = new ManageMessagesViewModel(new ObservableCollection<Message>(this.Context.dataEntities.Message));
                ManageMessagesWindow.DataContext = this.ManageMessagesViewModel;
            }
            
            ManageMessagesWindow.Show();
		}

		#endregion
	}
}
