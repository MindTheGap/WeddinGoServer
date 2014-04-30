using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PartyServer.ViewModels;
using System.Windows;
using System.Data.Entity.Validation;

namespace PartyServer.SaveToDB
{
    public class SaveToDBCommand : ICommand
    {
        public MainWindowViewModel Context { get; set; }

        public SaveToDBCommand(MainWindowViewModel context)
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
            if (this.Context != null)
            {
                try
                {
                    Context.SaveDBData();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Couldn't save DB Data! message: " + exception.InnerException);
                }
            }
		}

		#endregion
    }
}
