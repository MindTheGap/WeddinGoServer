using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using PartyServer.ViewModels;
using System.Collections;
using System;
using PartyServer.LogWindow;
using PartyServer.CommandClasses;
using PartyServer.ManageMessages;

namespace PartyServer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel MainViewModel { get; set; }

        public MainWindow()
        {
            MainViewModel = new MainWindowViewModel();

            InitializeComponent();

            mainGrid.DataContext = MainViewModel;

            Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to do that?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                result = MessageBox.Show("Do you want to save the DB Data?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        this.MainViewModel.SaveDBData();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Couldn't save DB Data! message: " + exception.InnerException);
                        result = MessageBox.Show("Do you still want to exit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.No)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }

            ManageLogCommand manageLogCommand = this.MainViewModel.ManageLogCommand as ManageLogCommand;
            if (manageLogCommand != null && manageLogCommand.ManageLogWindow != null)
            {
                manageLogCommand.ManageLogWindow.Close();
            }

            ManageUsersCommand manageUsersCommand = this.MainViewModel.ManageUsersCommand as ManageUsersCommand;
            if (manageUsersCommand != null && manageUsersCommand.ManageUsersWindow != null)
            {
                manageUsersCommand.ManageUsersWindow.Close();
            }

            ManageMessagesCommand manageMessagesCommand = this.MainViewModel.ManageMessagesCommand as ManageMessagesCommand;
            if (manageMessagesCommand != null && manageMessagesCommand.ManageMessagesWindow != null)
            {
                manageMessagesCommand.ManageMessagesWindow.Close();
            }
        }
    }
}
