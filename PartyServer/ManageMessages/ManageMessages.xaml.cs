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
using System.Globalization;

namespace PartyServer.ManageMessages
{
    /// <summary>
    /// Interaction logic for ManageMessages.xaml
    /// </summary>
    public partial class ManageMessages : Window
    {
        public ManageMessages()
        {
            InitializeComponent();
        }

        private void searchMessageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.Text = "";
                textBox.Opacity = 1.0;
            }
        }

        private void searchMessageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                ManageMessagesViewModel manageMessagesViewModel = mainGrid.DataContext as ManageMessagesViewModel;
                if (manageMessagesViewModel != null)
                {
                    if (this.MessagesDataGrid.ItemsSource == manageMessagesViewModel.AllMessagesCollection)
                    {
                        textBox.Text = "Search Name";
                        textBox.Opacity = 0.5;
                    }
                }
            }
        }

        private void searchMessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text.CompareTo("") == 0 || textBox.Text.CompareTo("Search Name") == 0)
                {
                    ManageMessagesViewModel manageMessagesViewModel = mainGrid.DataContext as ManageMessagesViewModel;
                    if (manageMessagesViewModel != null)
                    {
                        this.MessagesDataGrid.ItemsSource = manageMessagesViewModel.AllMessagesCollection;
                    }
                }
                else
                {
                    string txtOrig = textBox.Text;
                    string upper = txtOrig.ToUpper();
                    string lower = txtOrig.ToLower();


                    if (mainGrid.DataContext != null)
                    {
                        ManageMessagesViewModel manageMessagesViewModel = mainGrid.DataContext as ManageMessagesViewModel;
                        if (manageMessagesViewModel != null)
                        {
                            CultureInfo culture = CultureInfo.CurrentCulture;

                            var messagesFiltered = (from Msg in manageMessagesViewModel.AllMessagesCollection
                                                 let action = Msg.Action
                                                 let data = Msg.Data
                                                 let id = Msg.MessageID
                                                 where
                                                  culture.CompareInfo.IndexOf(action.ToString(), txtOrig, CompareOptions.IgnoreCase) >= 0

                                                  || culture.CompareInfo.IndexOf(data, txtOrig, CompareOptions.IgnoreCase) >= 0

                                                  || culture.CompareInfo.IndexOf(id.ToString(), txtOrig, CompareOptions.IgnoreCase) >= 0
                                                 select Msg).ToList();

                            this.MessagesDataGrid.ItemsSource = messagesFiltered;
                        }
                    }
                }
            }
        }
    }
}
