using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PartyServer.Helpers;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using PartyServer.CommandClasses;
using PartyServer.ManageMessages;
using System.Linq;
using PartyServer.DB;
using System.IO;
using Newtonsoft.Json;
using PartyServer.LogWindow;
using PartyServer.ThreadPoolManager;
using System.Windows.Threading;
using PartyServer.SaveToDB;
using PartyServer.PushMessage;

namespace PartyServer.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Properties

        public Thread Listener;

        public MainDBEntities1 dataEntities = new MainDBEntities1();

        public ThreadPoolManager.ThreadPoolManager ThreadPoolManager { get; set; }

        private int _totalMessages;

        public int TotalMessages
        {
            get { return _totalMessages; }
            set
            {
                _totalMessages = value;
                RaisePropertyChanged("TotalMessages");
            }
        }

        private int _messagesLastHour;

        public int MessagesLastHour
        {
            get { return _messagesLastHour; }
            set
            {
                _messagesLastHour = value;
                RaisePropertyChanged("MessagesLastHour");
            }
        }

        public Dispatcher UIDispatcher { get; set; }

        #endregion

        #region Commands

        private ICommand _manageUsersCommand;

        public ICommand ManageUsersCommand
        {
            get
            {
                if (_manageUsersCommand == null)
                    _manageUsersCommand = new ManageUsersCommand(this);

                return _manageUsersCommand;
            }
            set { _manageUsersCommand = value; }
        }

        private ICommand _manageMessagesCommand;

        public ICommand ManageMessagesCommand
        {
            get
            {
                if (_manageMessagesCommand == null)
                    _manageMessagesCommand = new ManageMessagesCommand(this);

                return _manageMessagesCommand;
            }
            set { _manageMessagesCommand = value; }
        }

        private ICommand _pushMessageCommand;

        public ICommand PushMessageCommand
        {
            get
            {
                if (_pushMessageCommand == null)
                    _pushMessageCommand = new PushMessageCommand(this);

                return _pushMessageCommand;
            }
            set { _pushMessageCommand = value; }
        }

        private ICommand _manageLogCommand;

        public ICommand ManageLogCommand
        {
            get
            {
                if (_manageLogCommand == null)
                    _manageLogCommand = new ManageLogCommand(this);

                return _manageLogCommand;
            }
            set { _manageLogCommand = value; }
        }

        private ICommand _saveToDBCommand;

        public ICommand SaveToDBCommand
        {
            get
            {
                if (_saveToDBCommand == null)
                    _saveToDBCommand = new SaveToDBCommand(this);

                return _saveToDBCommand;
            }
            set { _saveToDBCommand = value; }
        }

        #endregion

        #region Ctor

        public MainWindowViewModel()
        {
            try
            {
                this.ManageLogCommand = new ManageLogCommand(this);
                this.ThreadPoolManager = new ThreadPoolManager.ThreadPoolManager(this);

                UIDispatcher = Application.Current.Dispatcher;

                Listener = new Thread(() => RequestListener(new string[] { "http://*:4296/" }));
                Listener.Name = "ListenerThread";
                Listener.IsBackground = true;
                Listener.Start();
            }
            catch (Exception exception)
            {
                LogAddMessage("Main had an error: " + exception.Message);
            }
        }

        #endregion

        #region Log Helpers

        public void LogAddMessage(String message)
        {
            String strData = message;
            if (message.Length > 4000)
                strData = strData.Substring(0, 4000);

            Log addedLog = new Log() { Data = strData, Time = DateTime.Now.ToString() };
            if (this.dataEntities != null && this.dataEntities.Log != null)
                this.dataEntities.Log.Add(addedLog);
        }

        public void LogAddMessage(Log logMessage)
        {
            ManageLogCommand manageLogCommand = this.ManageLogCommand as ManageLogCommand;
            if (manageLogCommand != null && manageLogCommand.ManageLogViewModel != null && manageLogCommand.ManageLogViewModel.AllLogCollection != null)
            {
                UIDispatcher.BeginInvoke((Action)delegate()
                {
                    logMessage.Time = DateTime.Now.ToString();
                    manageLogCommand.ManageLogViewModel.AllLogCollection.Add(logMessage);
                    dataEntities.Log.Add(logMessage);
                }, null);
            }
        }

        #endregion Log Helpers

        #region DB Helpers

        public void SaveDBData()
        {
            dataEntities.SaveChanges();

            LogAddMessage("Saved DB Successfully!");
        }

        #endregion DB Helpers

        #region Listener

        public void RequestListener(string[] prefixes)
        {
            if (prefixes.Length == 0)
            {
                MessageBox.Show("Prefixes array is empty");
                return;
            }

            var listener = new HttpListener();

            foreach (string prefix in prefixes)
            {
                listener.Prefixes.Add(prefix);
            }

            listener.Start();

            while (true)
            {
                HttpListenerContext context = listener.GetContext();

                ListenerCallback(context);
            }
        }

       

        public void ListenerCallback(HttpListenerContext result)
        {
            if (result != null)
            {
                HttpListenerRequest request = result.Request;

                if (request.HasEntityBody)
                {
                    ThreadPoolManager.AddTask(result);

                    TotalMessages++;
                }
                else
                {
                    LogAddMessage("Got message with no entity body!");
                }
            }
            else
            {
                LogAddMessage("Got message with context null!");
            }
        }


        #endregion

    }
}