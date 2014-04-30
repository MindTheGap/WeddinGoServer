using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PartyServer.ViewModels;

namespace PartyServer.PushMessage
{
    public class PushMessageCommand : ICommand
    {
        public MainWindowViewModel Context { get; set; }

        public PushMessages PushMessagesWindow { get; set; }

        public PushMessagesViewModel PushMessagesViewModel { get; set; }

        public PushMessageCommand(MainWindowViewModel context)
		{
			this.Context = context;
            this.PushMessagesViewModel = new PushMessagesViewModel(context);
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
            if (PushMessagesWindow == null || PushMessagesWindow.IsActive == false)
            {
                PushMessagesWindow = new PushMessages();
                PushMessagesWindow.DataContext = this.PushMessagesViewModel;
            }
            
            PushMessagesWindow.Show();
		}

		#endregion
    }
}
