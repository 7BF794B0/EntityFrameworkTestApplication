using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using EntityFrameworkTestApplication.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace EntityFrameworkTestApplication
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow
    {
        private readonly List<int> _listOfFriends = new List<int>();
        private bool _redisIsStarted;

        #region getter/setter
        /// <value>First Name User.</value>
        public string FirstNameParam
        {
            set { txtFirstName.Text = value; }
        }

        /// <value>Last Name User.</value>
        public string LastNameParam
        {
            set { txtLastName.Text = value; }
        }

        /// <value>User Age.</value>
        public string AgeParam
        {
            set { txtAge.Text = value; }
        }

        /// <value>Country of residence user.</value>
        public string CountryParam
        {
            set { txtCountry.Text = value; }
        }

        /// <value>City user residence.</value>
        public string CityParam
        {
            set { txtCity.Text = value; }
        }

        /// <value>General information about the user.</value>
        public string BioParam
        {
            set { txtBio.Text = value; }
        }

        /// <value>User's friends.</value>
        public string FriendsParam
        {
            set { lstFriends.Items.Add(value); }
        }

        /// <value>User's friends list.</value>
        public int ListOfFriendsParam
        {
            set { _listOfFriends.Add(value); }
        }

        /// <value>Time.</value>
        public string TimeParam
        {
            set { lblTime.Content = value; }
        }

        /// <value>Do Radis hes Launched.</value>
        public bool StartedRedis
        {
            set { _redisIsStarted = value; }
        }
        #endregion

        #region ModalWindow
        // Methods used in filling information of a modal window.

        /// <summary>
        /// Enters the information about the user in Redis.
        /// </summary>
        /// <param name="db">Connecting with Redis.</param>
        /// <param name="user">Instance information about the user.</param>
        private static void RedisSetData(IDatabase db, IReadOnlyList<User> user)
        {
            // Serialization of User class using Json.
            var jsonValue = JsonConvert.SerializeObject(user);
            try
            {
                db.StringSet($"user:{user[0].Id}", jsonValue);
                // Life Time 10 minutes.
                db.KeyExpire($"user:{user[0].Id}", new TimeSpan(0, 10, 0));
            }
            catch (RedisException ex)
            {
                var swRedisExceptionLog = new StreamWriter("RedisExceptionLog.txt", true);
                swRedisExceptionLog.WriteLine("#########################################################################################");
                swRedisExceptionLog.WriteLine($"RedisException DateTime: {DateTime.Now}\n");
                swRedisExceptionLog.WriteLine($"RedisException Data: {ex.Data}\n");
                swRedisExceptionLog.WriteLine($"RedisException HelpLink: {ex.HelpLink}\n");
                swRedisExceptionLog.WriteLine($"RedisException HResult: {ex.HResult}\n");
                swRedisExceptionLog.WriteLine($"RedisException InnerException: {ex.InnerException}\n");
                swRedisExceptionLog.WriteLine($"RedisException Message: {ex.Message}\n");
                swRedisExceptionLog.WriteLine($"RedisException Source: {ex.Source}\n");
                swRedisExceptionLog.WriteLine($"RedisException StackTrace: {ex.StackTrace}\n");
                swRedisExceptionLog.WriteLine($"RedisException TargetSite: {ex.TargetSite}\n");
                swRedisExceptionLog.Close();
            }
        }

        /// <summary>
        /// Method of getting data from SQL.
        /// Called from the method RedisOrSQLStorage.
        /// </summary>
        /// <param name="queryUserStorage">Query our request.</param>
        /// <param name="db">Connecting with Redis.</param>
        /// <param name="redisIsStarted">The flag to check launched Redis.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static void GetDataFromSQL(IEnumerable<User> queryUserStorage, IDatabase db, bool redisIsStarted, InfoWindow frm)
        {
            var user = queryUserStorage.ToList();
            frm.FirstNameParam = user[0].First_Name;
            frm.LastNameParam = user[0].Last_Name;
            frm.AgeParam = user[0].Age.ToString();
            frm.BioParam = user[0].Bio;
            frm.CountryParam = user[0].Country;
            frm.CityParam = user[0].City;
            if (redisIsStarted)
                RedisSetData(db, user);
        }

        /// <summary>
        /// Method of getting data from Redis.
        /// Called from the method RedisOrSQLStorage.
        /// </summary>
        /// <param name="value">JSON-string.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static void GetDataFromRedis(string value, InfoWindow frm)
        {
            value = value.Substring(value.IndexOf("[", StringComparison.Ordinal) + 1);
            value = value.Substring(0, value.LastIndexOf("]", StringComparison.Ordinal));
            var u = JsonConvert.DeserializeObject<User>(value);

            frm.FirstNameParam = u.First_Name;
            frm.LastNameParam = u.Last_Name;
            frm.AgeParam = u.Age.ToString();
            frm.BioParam = u.Bio;
            frm.CountryParam = u.Country;
            frm.CityParam = u.City;
        }
        #region RedisOrSQL
        /// <summary>
        /// Check Method cached user in Redis or not.
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer (modal).
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">Instance a modal window.</param>
        private void RedisOrSQLStorage1(Storage1Context context, int userId, InfoWindow frm)
        {
            IDatabase db = null;
            if (_redisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, frm);
                    }
                    else
                    {
                        GetDataFromRedis(value, frm);
                    }
                }
            }
            else
            {
                var queryUserStorage = context.User.Where(x => x.Id == userId);
                GetDataFromSQL(queryUserStorage, db, false, frm);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not.
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer (modal).
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">Instance a modal window.</param>
        private void RedisOrSQLStorage2(Storage2Context context, int userId, InfoWindow frm)
        {
            IDatabase db = null;
            if (_redisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, frm);
                    }
                    else
                    {
                        GetDataFromRedis(value, frm);
                    }
                }
            }
            else
            {
                var queryUserStorage = context.User.Where(x => x.Id == userId);
                GetDataFromSQL(queryUserStorage, db, false, frm);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not.
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer (modal).
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">Instance a modal window.</param>
        private void RedisOrSQLStorage3(Storage3Context context, int userId, InfoWindow frm)
        {
            IDatabase db = null;
            if (_redisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, frm);
                    }
                    else
                    {
                        GetDataFromRedis(value, frm);
                    }
                }
            }
            else
            {
                var queryUserStorage = context.User.Where(x => x.Id == userId);
                GetDataFromSQL(queryUserStorage, db, false, frm);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not.
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer (modal).
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">Instance a modal window.</param>
        private void RedisOrSQLStorage4(Storage4Context context, int userId, InfoWindow frm)
        {
            IDatabase db = null;
            if (_redisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, frm);
                    }
                    else
                    {
                        GetDataFromRedis(value, frm);
                    }
                }
            }
            else
            {
                var queryUserStorage = context.User.Where(x => x.Id == userId);
                GetDataFromSQL(queryUserStorage, db, false, frm);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not.
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer (modal).
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">Instance a modal window.</param>
        private void RedisOrSQLStorage5(Storage5Context context, int userId, InfoWindow frm)
        {
            IDatabase db = null;
            if (_redisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, frm);
                    }
                    else
                    {
                        GetDataFromRedis(value, frm);
                    }
                }
            }
            else
            {
                var queryUserStorage = context.User.Where(x => x.Id == userId);
                GetDataFromSQL(queryUserStorage, db, false, frm);
            }
        }
        #endregion
        #region FindFriendsStorage
        /// <summary>
        /// Methods of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called from the methods GetUserInfoFromStorage.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static string FindFriendsStorage1(Storage1Context context, Friends f, InfoWindow frm)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            frm.ListOfFriendsParam = elementToAdd[0].Id;
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// Methods of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called from the methods GetUserInfoFromStorage.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static string FindFriendsStorage2(Storage2Context context, Friends f, InfoWindow frm)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            frm.ListOfFriendsParam = elementToAdd[0].Id;
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// Methods of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called from the methods GetUserInfoFromStorage.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static string FindFriendsStorage3(Storage3Context context, Friends f, InfoWindow frm)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            frm.ListOfFriendsParam = elementToAdd[0].Id;
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// Methods of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called from the methods GetUserInfoFromStorage.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static string FindFriendsStorage4(Storage4Context context, Friends f, InfoWindow frm)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            frm.ListOfFriendsParam = elementToAdd[0].Id;
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// Methods of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called from the methods GetUserInfoFromStorage.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static string FindFriendsStorage5(Storage5Context context, Friends f, InfoWindow frm)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            frm.ListOfFriendsParam = elementToAdd[0].Id;
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }
        #endregion
        #region ConnectToServer
        /// <summary>
        /// The connection method to Storage1.
        /// Called from an event handler lstFriends_MouseDoubleClick.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">Instance a modal window.</param>
        private void ConnectToServer1(Storage1Context context, int userId, InfoWindow frm)
        {
            RedisOrSQLStorage1(context, userId, frm);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    frm.FriendsParam = FindFriendsStorage1(context, f, frm);
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        frm.FriendsParam = FindFriendsStorage2(storage2Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        frm.FriendsParam = FindFriendsStorage3(storage3Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        frm.FriendsParam = FindFriendsStorage4(storage4Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        frm.FriendsParam = FindFriendsStorage5(storage5Context, f, frm);
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage2.
        /// Called from an event handler lstFriends_MouseDoubleClick.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">Instance a modal window.</param>
        private void ConnectToServer2(Storage2Context context, int userId, InfoWindow frm)
        {
            RedisOrSQLStorage2(context, userId, frm);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        frm.FriendsParam = FindFriendsStorage1(storage1Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    frm.FriendsParam = FindFriendsStorage2(context, f, frm);
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        frm.FriendsParam = FindFriendsStorage3(storage3Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        frm.FriendsParam = FindFriendsStorage4(storage4Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        frm.FriendsParam = FindFriendsStorage5(storage5Context, f, frm);
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage3.
        /// Called from an event handler lstFriends_MouseDoubleClick.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">Instance a modal window.</param>
        private void ConnectToServer3(Storage3Context context, int userId, InfoWindow frm)
        {
            RedisOrSQLStorage3(context, userId, frm);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        frm.FriendsParam = FindFriendsStorage1(storage1Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        frm.FriendsParam = FindFriendsStorage2(storage2Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    frm.FriendsParam = FindFriendsStorage3(context, f, frm);
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        frm.FriendsParam = FindFriendsStorage4(storage4Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        frm.FriendsParam = FindFriendsStorage5(storage5Context, f, frm);
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage4.
        /// Called from an event handler lstFriends_MouseDoubleClick.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">Instance a modal window.</param>
        private void ConnectToServer4(Storage4Context context, int userId, InfoWindow frm)
        {
            RedisOrSQLStorage4(context, userId, frm);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        frm.FriendsParam = FindFriendsStorage1(storage1Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        frm.FriendsParam = FindFriendsStorage2(storage2Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        frm.FriendsParam = FindFriendsStorage3(storage3Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    frm.FriendsParam = FindFriendsStorage4(context, f, frm);
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        frm.FriendsParam = FindFriendsStorage5(storage5Context, f, frm);
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage5.
        /// Called from an event handler lstFriends_MouseDoubleClick.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">Instance a modal window.</param>
        private void ConnectToServer5(Storage5Context context, int userId, InfoWindow frm)
        {
            RedisOrSQLStorage5(context, userId, frm);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        frm.FriendsParam = FindFriendsStorage1(storage1Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        frm.FriendsParam = FindFriendsStorage2(storage2Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        frm.FriendsParam = FindFriendsStorage3(storage3Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        frm.FriendsParam = FindFriendsStorage4(storage4Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    frm.FriendsParam = FindFriendsStorage5(context, f, frm);
                }
            }
        }
        #endregion
        #endregion

        public InfoWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event: Get detailed information about a friend.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void lstFriends_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            #region The code required for diagnosis - measures the time
            var spectator = new Stopwatch();
            spectator.Start();
            #endregion

            if (lstFriends.SelectedItem == null)
                return;

            var userId = _listOfFriends[lstFriends.SelectedIndex];
            var frm = new InfoWindow {ShowInTaskbar = false /* Do not show on the taskbar. */};

            if (userId <= 100000)
            {
                using (var context = new Storage1Context())
                {
                    ConnectToServer1(context, userId, frm);
                }
            }
            else if (userId > 100000 && userId < 200000)
            {
                using (var context = new Storage2Context())
                {
                    ConnectToServer2(context, userId, frm);
                }
            }
            else if (userId > 200000 && userId < 300000)
            {
                using (var context = new Storage3Context())
                {
                    ConnectToServer3(context, userId, frm);
                }
            }
            else if (userId > 300000 && userId < 400000)
            {
                using (var context = new Storage4Context())
                {
                    ConnectToServer4(context, userId, frm);
                }
            }
            else if (userId > 400000 && userId < 500000)
            {
                using (var context = new Storage5Context())
                {
                    ConnectToServer5(context, userId, frm);
                }
            }

            #region The code required for diagnosis - measures the time
            spectator.Stop();
            var ts = spectator.Elapsed;
            frm.TimeParam = $"Lead time: {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds/10:00}";
            #endregion
            frm.Show();
        }
    }
}
