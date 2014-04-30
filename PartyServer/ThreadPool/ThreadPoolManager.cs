using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.ObjectModel;
using PartyServer.ViewModels;
using System.Threading;
using PartyServer.DB;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Dynamic;

namespace PartyServer.ThreadPoolManager
{
    public class ThreadPoolManager : BaseViewModel
    {
        MainWindowViewModel MainWindowViewModel { get; set; }

        public DateTime TimerForMessagesLastHour { get; set; }

        private int _availableWorkerThreads;
    
        public int AvailableWorkerThreads
        {
            get { return _availableWorkerThreads; }
            set
            {
                _availableWorkerThreads = value;
                RaisePropertyChanged("AvailableWorkerThreads");
            }
        }

        private int _availableCompletionPortThreads;

        public int AvailableCompletionPortThreads
        {
            get { return _availableCompletionPortThreads; }
            set
            {
                _availableCompletionPortThreads = value;
                RaisePropertyChanged("AvailableCompletionPortThreads");
            }
        }

        private int _maxWorkerThreads;

        public int MaxWorkerThreads
        {
            get { return _maxWorkerThreads; }
            set
            {
                _maxWorkerThreads = value;
                RaisePropertyChanged("MaxWorkerThreads");
            }
        }

        private int _maxCompletionPortThreads;

        public int MaxCompletionPortThreads
        {
            get { return _maxCompletionPortThreads; }
            set
            {
                _maxCompletionPortThreads = value;
                RaisePropertyChanged("MaxCompletionPortThreads");
            }
        }

        public ThreadPoolManager(MainWindowViewModel mainWindowViewModel)
        {
            this.MainWindowViewModel = mainWindowViewModel;
            
            int maxWorkerThreads, maxCompletionPortThreads;
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
            this.MaxWorkerThreads = maxWorkerThreads;
            this.MaxCompletionPortThreads = maxCompletionPortThreads;

            TimerForMessagesLastHour = DateTime.Now;
        }

        public void AddTask(HttpListenerContext context)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolWorkerCallback), context);

            DateTime currentDateTime = DateTime.Now;
            if (currentDateTime.Subtract(TimerForMessagesLastHour).Hours >= 1)
            {
                TimerForMessagesLastHour = currentDateTime;
                this.MainWindowViewModel.MessagesLastHour = 0;
            }
            else
            {
                this.MainWindowViewModel.MessagesLastHour++;
            }

            int availableWorkerThreads, availableCompletionPortThreads;
            ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableCompletionPortThreads);
            this.AvailableWorkerThreads = availableWorkerThreads;
            this.AvailableCompletionPortThreads = availableCompletionPortThreads;
        }


        private void LogAddMessage(String message)
        {
            if (MainWindowViewModel != null)
            {
                MainWindowViewModel.LogAddMessage(message);
            }
        }

        private void LogAddMessage(Log message)
        {
            if (MainWindowViewModel != null)
            {
                MainWindowViewModel.LogAddMessage(message);
            }
        }

        private bool UserExists(List<Member> usersList, string userEmailAddress, out Member userOut)
        {
            if (userEmailAddress == null)
            {
                userOut = null;
                return false;
            }
            
            foreach (var user in usersList)
            {
                if (user.Email == userEmailAddress)
                {
                    userOut = user;
                    return true;
                }
            }

            userOut = null;

            return false;
        }

        private void ThreadPoolWorkerCallback(object state)
        {
            HttpListenerContext context = state as HttpListenerContext;
            HttpListenerRequest request = context.Request;
            if (request != null && request.HasEntityBody)
            {
                using (Stream body = request.InputStream) // here we have data
                {
                    using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
                    {
                        string strData = reader.ReadToEnd();

                        dynamic dynamicObject = JsonConvert.DeserializeObject(strData);

                        Nullable<PartyServer.Helpers.Helpers.MessageTypeFromClient> type = dynamicObject.type;
                        if (type != null)
                        {
                            switch (type)
                            {
                                case Helpers.Helpers.MessageTypeFromClient.SearchResults:

                                    HandleSearchResultsMessage(dynamicObject, context);

                                    break;

                                case Helpers.Helpers.MessageTypeFromClient.GetAllJoinedWeddings:

                                    HandleGetAllJoinedWeddingsMessage(dynamicObject, context);
                                    
                                    break;

                                case Helpers.Helpers.MessageTypeFromClient.LikeGreeting:

                                    HandleLikeGreetingMessage(dynamicObject, context);
                                    
                                    break;

                                case Helpers.Helpers.MessageTypeFromClient.Registration:

                                    HandleRegistrationMessage(dynamicObject, context);

                                    break;


                               

                                //case Helpers.Helpers.MessageTypeFromClient.RetreiveLastMessages:

                                //    HandleRetreiveLastMessagesMessage(strData, context);

                                //    break;



                                //case Helpers.Helpers.MessageTypeFromClient.RetreiveTopHits:



                                //    break;


                                //case Helpers.Helpers.MessageTypeFromClient.PushGreeting:

                                //    HandlePushGreetingMessage(strData, context);

                                //    break;
                            }
                        }
                    }
                }
            }
        }

        //private void HandlePushGreetingMessage(string strData, HttpListenerContext context)
        //{
        //     // Translate data bytes to a Message class using JSON.
        //    MessageFromClient inputMessage = JsonConvert.DeserializeObject<MessageFromClient>(strData);

        //    if (inputMessage != null && inputMessage.Data != null)
        //    {
        //        Member userOut = null;
        //        if (UserExists(MainWindowViewModel.dataEntities.Member.ToList(), inputMessage.Email, out userOut) == true)
        //        {
        //            Debug.Assert(userOut != null);

        //            if (userOut.Is_Blocked == false)
        //            {
        //                LogAddMessage("Pushing greeting from user: " + inputMessage.UserFirstName + " " + inputMessage.UserLastName);

        //                var greeting = new Greeting();
        //                greeting.Text = inputMessage.Data;
        //                MainWindowViewModel.dataEntities.Greeting.Add(greeting);
        //                MainWindowViewModel.SaveDBData();

        //                var message = new MessageToClient(Helpers.Helpers.MessageTypeToClient.AOK);
        //                SendMessage(context, message);
        //            }
        //        }
        //    }
        //}

        //private void HandleRetreiveLastMessagesMessage(string strData, HttpListenerContext context)
        //{
        //     // Translate data bytes to a Message class using JSON.
        //    MessageFromClient inputMessage = JsonConvert.DeserializeObject<MessageFromClient>(strData);

        //    if (inputMessage != null)
        //    {
        //        Member userOut = null;
        //        List<Member> list = MainWindowViewModel.dataEntities.Member.ToList();
        //        if (UserExists(list, inputMessage.Email, out userOut) == true)
        //        {
        //            Debug.Assert(userOut != null);

        //            if (userOut.Is_Blocked == false)
        //            {
        //                LogAddMessage("Sending last messages to user: " + inputMessage.UserFirstName + " " + inputMessage.UserLastName);

        //                var message = new MessageToClient(Helpers.Helpers.MessageTypeToClient.SentLastMessages);
        //                List<Greeting> greetingList = MainWindowViewModel.dataEntities.Greeting.ToList();
        //                foreach (var item in greetingList)
        //                {
        //                    item.Wedding = null;
        //                }
        //                List<Photo> photoList = MainWindowViewModel.dataEntities.Photo.ToList();
        //                foreach (var item in photoList)
        //                {
        //                    item.Wedding = null;
        //                }
        //                message.GreetingsList = greetingList;
        //                message.PhotosList = photoList;
        //                SendMessage(context, message);
        //            }
        //        }
        //    }
        //}
        
        private void HandleGetAllJoinedWeddingsMessage(dynamic dynamicObject, HttpListenerContext context)
        {
            try
            {
                dynamic sendFlexible = new ExpandoObject();
                sendFlexible.Type = Helpers.Helpers.MessageTypeToClient.GetAllJoinedWeddings;
                var weddings = from wedding in MainWindowViewModel.dataEntities.Wedding
                               where wedding.Member.First_Name.Contains("Chen") || wedding.Member1.First_Name.Contains("Chen") || wedding.Member.First_Name.Contains("Eti") || wedding.Member1.First_Name.Contains("Eti")
                               select wedding;

                List<ExpandoObject> flexibleWeddingList = new List<ExpandoObject>();
                foreach (var weddingLinq in weddings)
                {
                    dynamic flexibleWedding = new ExpandoObject();
                    flexibleWedding.BrideFullName = weddingLinq.Member.First_Name + " " + weddingLinq.Member.Last_Name;
                    flexibleWedding.GroomFullName = weddingLinq.Member1.First_Name + " " + weddingLinq.Member1.Last_Name;
                    flexibleWedding.Date = weddingLinq.Date;
                    flexibleWedding.Place = weddingLinq.Place;
                    if (weddingLinq.Photo != null)
                        flexibleWedding.Image = weddingLinq.Photo.Image_Path;
                    flexibleWeddingList.Add(flexibleWedding);
                }

                sendFlexible.Weddings = flexibleWeddingList;

                SendMessage(context, sendFlexible);
            }
            catch (Exception exception)
            {
                LogAddMessage("Message has exception: " + exception.Message + ". InnerMessage: " + exception.InnerException);
            }
        }

        private void HandleLikeGreetingMessage(dynamic dynamicObject, HttpListenerContext context)
        {
            try
            {
                Member userOut = null;
                List<Member> membersList = MainWindowViewModel.dataEntities.Member.ToList();
                string email = dynamicObject.Email;
                if (UserExists(membersList, email, out userOut) == true)
                {
                    Debug.Assert(userOut != null);

                    if (userOut.Is_Blocked == true)
                    {
                        LogAddMessage("Received message from blocked user: " + dynamicObject.UserFirstName + " " + dynamicObject.UserLastName + ". ignoring.");

                        return;
                    }

                    int? greetingId = dynamicObject.GreetingId;
                    if (greetingId != null)
                    {
                        foreach (var greeting in this.MainWindowViewModel.dataEntities.Greeting)
                        {
                            if (greeting.Greeting_ID == (int)greetingId)
                            {
                                greeting.Like.Add(new Like() { Greeting_ID = greetingId, Member_ID = userOut.Member_ID });
                                
                                dynamic sendFlexible = new ExpandoObject();
                                sendFlexible.Type = Helpers.Helpers.MessageTypeToClient.AOK;

                                SendMessage(context, sendFlexible);

                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogAddMessage("Message has exception: " + exception.Message + ". InnerMessage: " + exception.InnerException);
            }
        }


        private void HandleSearchResultsMessage(dynamic dynamicObject, HttpListenerContext context)
        {
            try
            {
                dynamic sendFlexible = new ExpandoObject();
                sendFlexible.Type = Helpers.Helpers.MessageTypeToClient.AOK;
                var weddings = from wedding in MainWindowViewModel.dataEntities.Wedding
                               where wedding.Member.First_Name.Contains("Chen") || wedding.Member1.First_Name.Contains("Chen") || wedding.Member.First_Name.Contains("Eti") || wedding.Member1.First_Name.Contains("Eti")
                               select wedding;

                List<ExpandoObject> flexibleWeddingList = new List<ExpandoObject>();
                foreach (var weddingLinq in weddings.Take(10))
                {
                    dynamic flexibleWedding = new ExpandoObject();
                    flexibleWedding.BrideFullName = weddingLinq.Member.First_Name + " " + weddingLinq.Member.Last_Name;
                    flexibleWedding.GroomFullName = weddingLinq.Member1.First_Name + " " + weddingLinq.Member1.Last_Name;
                    flexibleWedding.Date = weddingLinq.Date;
                    flexibleWedding.Place = weddingLinq.Place;
                    if (weddingLinq.Photo != null)
                        flexibleWedding.Image = weddingLinq.Photo.Image_Path;
                    flexibleWeddingList.Add(flexibleWedding);
                }

                sendFlexible.Results = flexibleWeddingList;

                SendMessage(context, sendFlexible);
            }
            catch (Exception exception)
            {
                LogAddMessage("Message has exception: " + exception.Message + ". InnerMessage: " + exception.InnerException);
            }
        }

        private void HandleRegistrationMessage(dynamic dynamicObject, HttpListenerContext context)
        {
            try
            {
                if (dynamicObject != null)
                {
                    Member userOut = null;
                    if (UserExists(MainWindowViewModel.dataEntities.Member.ToList(), dynamicObject.Email, out userOut) == true)
                    {
                        Debug.Assert(userOut != null);

                        if (userOut.Is_Blocked == true)
                        {
                            LogAddMessage("Received message from blocked user: " + dynamicObject.UserFirstName + " " + dynamicObject.UserLastName + ". ignoring.");

                            dynamic sendFlexible = new ExpandoObject();
                            sendFlexible.Type = Helpers.Helpers.MessageTypeToClient.Error;
                            sendFlexible.ErrorType = Helpers.Helpers.ErrorType.UserAlreadyExists;

                            SendMessage(context, sendFlexible);

                            return;
                        }
                    }
                    else // user doesn't exist
                    {
                        if (dynamicObject.UserFirstName != null && dynamicObject.UserLastName != null)
                        {
                            LogAddMessage("Adding user (" + dynamicObject.UserFirstName + " " + dynamicObject.UserLastName + ")!");
                            MainWindowViewModel.dataEntities.Member.Add(new Member()
                            {
                                First_Name = dynamicObject.UserFirstName,
                                Last_Name = dynamicObject.UserLastName,
                                Is_Blocked = false,
                                Email = dynamicObject.Email,
                                Token_ID = dynamicObject.DeviceToken,
                                Permission_ID = 5
                            });
                            MainWindowViewModel.SaveDBData();

                            dynamic sendFlexible = new ExpandoObject();
                            sendFlexible.Type = Helpers.Helpers.MessageTypeToClient.AOK;

                            SendMessage(context, sendFlexible);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogAddMessage("Message has exception: " + exception.Message + ". InnerMessage: " + exception.InnerException);
            }
        }

        private void SendMessage(HttpListenerContext context, dynamic dynamicObject)
        {
            // Obtain a response object.
            HttpListenerResponse response = context.Response;

            string responseString = JsonConvert.SerializeObject(dynamicObject);

            LogAddMessage(responseString);

            // Construct a response.
            byte[] buffer = Encoding.Unicode.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            response.ContentEncoding = Encoding.Unicode;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
        }
    }
}
