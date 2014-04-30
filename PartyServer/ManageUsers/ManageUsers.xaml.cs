using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PartyServer.ManageUsers;
using System.Collections.ObjectModel;
using PartyServer.DB;
using PartyServer.ViewModels;
using System.Globalization;

namespace PartyServer.Views
{
    /// <summary>
    /// Interaction logic for ManageUsers.xaml
    /// </summary>
    public partial class ManageUsers : Window
    {
        public ManageUsers()
        {
            InitializeComponent();
        }

        private void searchUserTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.Text = "";
                textBox.Opacity = 1.0;
            }
        }

        private void searchUserTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                ManageUsersViewModel manageUsersViewModel = mainGrid.DataContext as ManageUsersViewModel;
                if (manageUsersViewModel != null)
                {
                    if (this.UserDataGrid.ItemsSource == manageUsersViewModel.AllUsersCollection)
                    {
                        textBox.Text = "Search Name";
                        textBox.Opacity = 0.5;
                    }
                }
            }
        }

        private void searchUserTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text.CompareTo("") == 0 || textBox.Text.CompareTo("Search Name") == 0)
                {
                    ManageUsersViewModel manageUsersViewModel = mainGrid.DataContext as ManageUsersViewModel;
                    if (manageUsersViewModel != null)
                    {
                        this.UserDataGrid.ItemsSource = manageUsersViewModel.AllUsersCollection;
                    }
                }
                else
                {
                    string txtOrig = textBox.Text;
                    string upper = txtOrig.ToUpper();
                    string lower = txtOrig.ToLower();


                    if (mainGrid.DataContext != null)
                    {
                        ManageUsersViewModel manageUsersViewModel = mainGrid.DataContext as ManageUsersViewModel;
                        if (manageUsersViewModel != null)
                        {
                            CultureInfo culture = CultureInfo.CurrentCulture;

                            var usersFiltered = (from Usr in manageUsersViewModel.AllUsersCollection
                                                let fname = Usr.First_Name
                                                let lname = Usr.Last_Name
                                                let id = Usr.Member_ID
                                                let email = Usr.Email
                                                where
                                                 culture.CompareInfo.IndexOf(fname, txtOrig, CompareOptions.IgnoreCase) >= 0

                                                 || culture.CompareInfo.IndexOf(lname, txtOrig, CompareOptions.IgnoreCase) >= 0

                                                 || culture.CompareInfo.IndexOf(id.ToString(), txtOrig, CompareOptions.IgnoreCase) >= 0

                                                 || culture.CompareInfo.IndexOf(email, txtOrig, CompareOptions.IgnoreCase) >= 0
                                                select Usr).ToList();

                            this.UserDataGrid.ItemsSource = usersFiltered;
                        }
                    }
                }
            }
        }
    }
}
