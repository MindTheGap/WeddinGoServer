using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PartyServer.ViewModels;
using System.Windows.Input;
using PartyServer.Helpers;
using MoonAPNS;
using System.IO;

namespace PartyServer.PushMessage
{
    public class PushMessagesViewModel : BaseViewModel
    {
        public PushMessagesViewModel(MainWindowViewModel MainWindowViewModel)
        {
            this.MainWindowViewModel = MainWindowViewModel;

            PushButtonCommand = new RelayCommand(PushButtonExecute, PushButtonCanExecute);
        }

        public MainWindowViewModel MainWindowViewModel { get; set; }

        public String PushMessageText { get; set; }

        public ICommand PushButtonCommand
        {
            get;
            internal set;
        }

        private bool PushButtonCanExecute(object obj)
        {
            return !(this.PushMessageText == null || this.PushMessageText.Length == 0);
        }

        public void PushButtonExecute(object obj)
        {
            //var payload1 = new NotificationPayload("DeviceToken", this.PushMessageText, 1, "default");
            //payload1.AddCustom("CustomKey", "CustomValue");

            //var payload2 = new NotificationPayload("DeviceToken", this.PushMessageText, 1, "default");
            //payload2.AddCustom("CustomKey2", "CustomValue2");

            //var payload3 = new NotificationPayload("DeviceToken", this.PushMessageText, 1, "default");
            //payload3.AddCustom("CustomKey3", "CustomValue3");

            //var notificationList = new List { payload1, payload2, payload3 };

            //if (File.Exists(@"C:\CertificatesPush.p12") == true)
            //{
            //    // prompt for a password
            //    var push = new PushNotification(true, @"C:\CertificatesPush.p12", "");
            //    var feedBackList = push.GetFeedBack();

            //    var rejected = push.SendToApple(notificationList);
            //}
        }
    }
}
