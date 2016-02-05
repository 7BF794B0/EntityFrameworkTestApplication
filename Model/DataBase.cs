using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using Newtonsoft.Json;
using EntityFrameworkTestApplication.Models;

namespace EntityFrameworkTestApplication.Model
{
    /// <summary>
    /// The methods needed to fill information about the user.
    /// </summary>
    class DataBase
    {
        /// <summary>
        /// Enters the information about the user in Redis.
        /// </summary>
        /// <param name="db">Connecting with Redis.</param>
        /// <param name="user">Instance information about the user.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        private static void RedisSetData(IDatabase db, IReadOnlyList<User> user, Logging logging)
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
                logging.ProcessingException(ex);
            }
        }

        /// <summary>
        /// Method of getting data from SQL.
        /// Called from the method RedisOrSQLStorage.
        /// </summary>
        /// <param name="queryUserStorage">Query our request.</param>
        /// <param name="db">Connecting with Redis.</param>
        /// <param name="redisIsStarted">The flag to check launched Redis.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        private static void GetDataFromSQL(IEnumerable<User> queryUserStorage, IDatabase db, bool redisIsStarted, Logging logging, View.View view, System.Windows.Controls.TextBox[] textboxs)
        {
            var user = queryUserStorage.ToList();
            // Filling fields from SQL.
            view.ChangeTextIntoTextBox(textboxs, user[0]);
            // Caching in Redis.
            if (redisIsStarted)
                RedisSetData(db, user, logging);
        }

        /// <summary>
        /// Method of getting data from Redis.
        /// Called from the method RedisOrSQLStorage.
        /// </summary>
        /// <param name="value">JSON-string.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        private static void GetDataFromRedis(string value, View.View view, System.Windows.Controls.TextBox[] textboxs)
        {
            // Convert Json.
            value = value.Substring(value.IndexOf("[", StringComparison.Ordinal) + 1);
            value = value.Substring(0, value.LastIndexOf("]", StringComparison.Ordinal));
            // Deserializing JSON-string.
            var u = JsonConvert.DeserializeObject<User>(value);
            // Filling in the fields of Redis.
            view.ChangeTextIntoTextBox(textboxs, u);
        }

        /// <summary>
        /// Method of getting data from SQL.
        /// Called from the method RedisOrSQLStorage.
        /// </summary>
        /// <param name="queryUserStorage">Query our request.</param>
        /// <param name="db">Connecting with Redis.</param>
        /// <param name="redisIsStarted">The flag to check launched Redis.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static void GetDataFromSQL(IEnumerable<User> queryUserStorage, IDatabase db, bool redisIsStarted, Logging logging, InfoWindow frm)
        {
            var user = queryUserStorage.ToList();
            frm.FirstNameParam = user[0].First_Name;
            frm.LastNameParam = user[0].Last_Name;
            frm.AgeParam = user[0].Age.ToString();
            frm.BioParam = user[0].Bio;
            frm.CountryParam = user[0].Country;
            frm.CityParam = user[0].City;
            if (redisIsStarted)
                RedisSetData(db, user, logging);
        }

        /// <summary>
        /// Method of getting data from Redis.
        /// Called from the method RedisOrSQLStorage (modal).
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

        /// <summary>
        /// Check Method cached user in Redis or not.
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        private static void RedisOrSQLStorage1(Storage1Context context, int userId, Services services, Logging logging, View.View view, System.Windows.Controls.TextBox[] textboxs)
        {
            IDatabase db = null;
            // If Redis is enabled.
            if (services.RedisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    // If Redis is on and there is no required us to record.
                    if (string.IsNullOrEmpty(value))
                    {
                        // Sample.
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        // Take data from SQL.
                        GetDataFromSQL(queryUserStorage, db, true, logging, view, textboxs);
                    }
                    // If Redis is turned on and the correct recording us there.
                    else
                    {
                        // Take the data from the Redis.
                        GetDataFromRedis(value, view, textboxs);
                    }
                }
            }
            // If Redis is not enabled then take out the data from SQL.
            else
            {
                // Sample.
                var queryUserStorage = context.User.Where(x => x.Id == userId);
                // Take data from SQL.
                GetDataFromSQL(queryUserStorage, db, false, logging, view, textboxs);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not.
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        private static void RedisOrSQLStorage2(Storage2Context context, int userId, Services services, Logging logging, View.View view, System.Windows.Controls.TextBox[] textboxs)
        {
            IDatabase db = null;
            if (services.RedisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, logging, view, textboxs);
                    }
                    else
                    {
                        GetDataFromRedis(value, view, textboxs);
                    }
                }
            }
            else
            {
                var queryUserStorage = context.User.Where(x => x.Id == userId);
                GetDataFromSQL(queryUserStorage, db, false, logging, view, textboxs);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not.
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        private static void RedisOrSQLStorage3(Storage3Context context, int userId, Services services, Logging logging, View.View view, System.Windows.Controls.TextBox[] textboxs)
        {
            IDatabase db = null;
            if (services.RedisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, logging, view, textboxs);
                    }
                    else
                    {
                        GetDataFromRedis(value, view, textboxs);
                    }
                }
            }
            else
            {
                var queryUserStorage = context.User.Where(x => x.Id == userId);
                GetDataFromSQL(queryUserStorage, db, false, logging, view, textboxs);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not.
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        private static void RedisOrSQLStorage4(Storage4Context context, int userId, Services services, Logging logging, View.View view, System.Windows.Controls.TextBox[] textboxs)
        {
            IDatabase db = null;
            if (services.RedisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, logging, view, textboxs);
                    }
                    else
                    {
                        GetDataFromRedis(value, view, textboxs);
                    }
                }
            }
            else
            {
                var queryUserStorage = context.User.Where(x => x.Id == userId);
                GetDataFromSQL(queryUserStorage, db, false, logging, view, textboxs);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not.
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        private static void RedisOrSQLStorage5(Storage5Context context, int userId, Services services, Logging logging, View.View view, System.Windows.Controls.TextBox[] textboxs)
        {
            IDatabase db = null;
            if (services.RedisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, logging, view, textboxs);
                    }
                    else
                    {
                        GetDataFromRedis(value, view, textboxs);
                    }
                }
            }
            else
            {
                var queryUserStorage = context.User.Where(x => x.Id == userId);
                GetDataFromSQL(queryUserStorage, db, false, logging, view, textboxs);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not (modal).
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer (modal).
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static void RedisOrSQLStorage1(Storage1Context context, int userId, Services services, Logging logging, InfoWindow frm)
        {
            IDatabase db = null;
            if (services.RedisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, logging, frm);
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
                GetDataFromSQL(queryUserStorage, db, false, logging, frm);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not (modal).
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer (modal).
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static void RedisOrSQLStorage2(Storage2Context context, int userId, Services services, Logging logging, InfoWindow frm)
        {
            IDatabase db = null;
            if (services.RedisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, logging, frm);
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
                GetDataFromSQL(queryUserStorage, db, false, logging, frm);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not (modal).
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer (modal).
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static void RedisOrSQLStorage3(Storage3Context context, int userId, Services services, Logging logging, InfoWindow frm)
        {
            IDatabase db = null;
            if (services.RedisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, logging, frm);
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
                GetDataFromSQL(queryUserStorage, db, false, logging, frm);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not (modal).
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer (modal).
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static void RedisOrSQLStorage4(Storage4Context context, int userId, Services services, Logging logging, InfoWindow frm)
        {
            IDatabase db = null;
            if (services.RedisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, logging, frm);
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
                GetDataFromSQL(queryUserStorage, db, false, logging, frm);
            }
        }

        /// <summary>
        /// Check Method cached user in Redis or not (modal).
        /// If the user is not cached in Redis then its cache.
        /// Called from the method ConnectToServer (modal).
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="frm">Instance a modal window.</param>
        private static void RedisOrSQLStorage5(Storage5Context context, int userId, Services services, Logging logging, InfoWindow frm)
        {
            IDatabase db = null;
            if (services.RedisIsStarted)
            {
                using (var redisClient = ConnectionMultiplexer.Connect("localhost"))
                {
                    db = redisClient.GetDatabase();
                    string value = db.StringGet($"user:{userId}");

                    if (string.IsNullOrEmpty(value))
                    {
                        var queryUserStorage = context.User.Where(x => x.Id == userId);
                        GetDataFromSQL(queryUserStorage, db, true, logging, frm);
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
                GetDataFromSQL(queryUserStorage, db, false, logging, frm);
            }
        }

        /// <summary>
        /// The connection method to Storage1.
        /// Called from an event handler btnConnected_Click.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="listOfFriends">User's friends list.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="friend">User friend.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        /// <param name="lstFriends">ListBox control where your friends list will be added.</param>
        public void ConnectToServer1(Storage1Context context, int userId, List<int> listOfFriends, Services services, Logging logging, Friend friend, View.View view, System.Windows.Controls.TextBox[] textboxs, System.Windows.Controls.ListBox lstFriends)
        {
            RedisOrSQLStorage1(context, userId, services, logging, view, textboxs);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    lstFriends.Items.Add(friend.FindFriendsStorage1(context, f, listOfFriends));
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage2(storage2Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage3(storage3Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage4(storage4Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage5(storage5Context, f, listOfFriends));
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage2.
        /// Called from an event handler btnConnected_Click.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="listOfFriends">User's friends list.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="friend">User friend.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        /// <param name="lstFriends">ListBox control where your friends list will be added.</param>
        public void ConnectToServer2(Storage2Context context, int userId, List<int> listOfFriends, Services services, Logging logging, Friend friend, View.View view, System.Windows.Controls.TextBox[] textboxs, System.Windows.Controls.ListBox lstFriends)
        {
            RedisOrSQLStorage2(context, userId, services, logging, view, textboxs);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage1(storage1Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    lstFriends.Items.Add(friend.FindFriendsStorage2(context, f, listOfFriends));
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage3(storage3Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage4(storage4Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage5(storage5Context, f, listOfFriends));
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage3.
        /// Called from an event handler btnConnected_Click.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="listOfFriends">User's friends list.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="friend">User friend.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        /// <param name="lstFriends">ListBox control where your friends list will be added.</param>
        public void ConnectToServer3(Storage3Context context, int userId, List<int> listOfFriends, Services services, Logging logging, Friend friend, View.View view, System.Windows.Controls.TextBox[] textboxs, System.Windows.Controls.ListBox lstFriends)
        {
            RedisOrSQLStorage3(context, userId, services, logging, view, textboxs);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage1(storage1Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage2(storage2Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    lstFriends.Items.Add(friend.FindFriendsStorage3(context, f, listOfFriends));
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage4(storage4Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage5(storage5Context, f, listOfFriends));
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage4.
        /// Called from an event handler btnConnected_Click.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="listOfFriends">User's friends list.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="friend">User friend.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        /// <param name="lstFriends">ListBox control where your friends list will be added.</param>
        public void ConnectToServer4(Storage4Context context, int userId, List<int> listOfFriends, Services services, Logging logging, Friend friend, View.View view, System.Windows.Controls.TextBox[] textboxs, System.Windows.Controls.ListBox lstFriends)
        {
            RedisOrSQLStorage4(context, userId, services, logging, view, textboxs);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage1(storage1Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage2(storage2Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage3(storage3Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    lstFriends.Items.Add(friend.FindFriendsStorage4(context, f, listOfFriends));
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage5(storage5Context, f, listOfFriends));
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage5.
        /// Called from an event handler btnConnected_Click.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="listOfFriends">User's friends list.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="friend">User friend.</param>
        /// <param name="view">View class object to display the changes.</param>
        /// <param name="textboxs">An array of text fields to fill.</param>
        /// <param name="lstFriends">ListBox control where your friends list will be added.</param>
        public void ConnectToServer5(Storage5Context context, int userId, List<int> listOfFriends, Services services, Logging logging, Friend friend, View.View view, System.Windows.Controls.TextBox[] textboxs, System.Windows.Controls.ListBox lstFriends)
        {
            RedisOrSQLStorage5(context, userId, services, logging, view, textboxs);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage1(storage1Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage2(storage2Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage3(storage3Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        lstFriends.Items.Add(friend.FindFriendsStorage4(storage4Context, f, listOfFriends));
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    lstFriends.Items.Add(friend.FindFriendsStorage5(context, f, listOfFriends));
                }
            }
        }

        /// <summary>
        /// The connection method to Storage1 (modal).
        /// Called from an event handler lstFriends_MouseDoubleClick.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="friend">User friend.</param>
        /// <param name="lstFriends">ListBox control where your friends list will be added.</param>
        /// <param name="frm">Instance a modal window.</param>
        public void ConnectToServer1(Storage1Context context, int userId, Services services, Logging logging, Friend friend, System.Windows.Controls.ListBox lstFriends, InfoWindow frm)
        {
            RedisOrSQLStorage1(context, userId, services, logging, frm);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    frm.FriendsParam = friend.FindFriendsStorage1(context, f, frm);
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage2(storage2Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage3(storage3Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage4(storage4Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage5(storage5Context, f, frm);
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage2 (modal).
        /// Called from an event handler lstFriends_MouseDoubleClick.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="friend">User friend.</param>
        /// <param name="lstFriends">ListBox control where your friends list will be added.</param>
        /// <param name="frm">Instance a modal window.</param>
        public void ConnectToServer2(Storage2Context context, int userId, Services services, Logging logging, Friend friend, System.Windows.Controls.ListBox lstFriends, InfoWindow frm)
        {
            RedisOrSQLStorage2(context, userId, services, logging, frm);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage1(storage1Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    frm.FriendsParam = friend.FindFriendsStorage2(context, f, frm);
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage3(storage3Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage4(storage4Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage5(storage5Context, f, frm);
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage3 (modal).
        /// Called from an event handler lstFriends_MouseDoubleClick.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="friend">User friend.</param>
        /// <param name="lstFriends">ListBox control where your friends list will be added.</param>
        /// <param name="frm">Instance a modal window.</param>
        public void ConnectToServer3(Storage3Context context, int userId, Services services, Logging logging, Friend friend, System.Windows.Controls.ListBox lstFriends, InfoWindow frm)
        {
            RedisOrSQLStorage3(context, userId, services, logging, frm);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage1(storage1Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage2(storage2Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    frm.FriendsParam = friend.FindFriendsStorage3(context, f, frm);
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage4(storage4Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage5(storage5Context, f, frm);
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage4 (modal).
        /// Called from an event handler lstFriends_MouseDoubleClick.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="friend">User friend.</param>
        /// <param name="lstFriends">ListBox control where your friends list will be added.</param>
        /// <param name="frm">Instance a modal window.</param>
        public void ConnectToServer4(Storage4Context context, int userId, Services services, Logging logging, Friend friend, System.Windows.Controls.ListBox lstFriends, InfoWindow frm)
        {
            RedisOrSQLStorage4(context, userId, services, logging, frm);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage1(storage1Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage2(storage2Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage3(storage3Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    frm.FriendsParam = friend.FindFriendsStorage4(context, f, frm);
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    using (var storage5Context = new Storage5Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage5(storage5Context, f, frm);
                    }
                }
            }
        }

        /// <summary>
        /// The connection method to Storage5 (modal).
        /// Called from an event handler lstFriends_MouseDoubleClick.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="services">Services class object to check - whether our services work.</param>
        /// <param name="logging">Logging class object to create the log.</param>
        /// <param name="friend">User friend.</param>
        /// <param name="lstFriends">ListBox control where your friends list will be added.</param>
        /// <param name="frm">Instance a modal window.</param>
        public void ConnectToServer5(Storage5Context context, int userId, Services services, Logging logging, Friend friend, System.Windows.Controls.ListBox lstFriends, InfoWindow frm)
        {
            RedisOrSQLStorage5(context, userId, services, logging, frm);

            var queryFriendsStorage = context.Friends.Where(x => x.User_Id == userId);
            var friends = queryFriendsStorage.ToList();
            foreach (var f in friends)
            {
                if (f.Friend_Id < 100000)
                {
                    using (var storage1Context = new Storage1Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage1(storage1Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 100000 && f.Friend_Id < 200000)
                {
                    using (var storage2Context = new Storage2Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage2(storage2Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 200000 && f.Friend_Id < 300000)
                {
                    using (var storage3Context = new Storage3Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage3(storage3Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 300000 && f.Friend_Id < 400000)
                {
                    using (var storage4Context = new Storage4Context())
                    {
                        frm.FriendsParam = friend.FindFriendsStorage4(storage4Context, f, frm);
                    }
                }
                else if (f.Friend_Id > 400000 && f.Friend_Id < 500000)
                {
                    frm.FriendsParam = friend.FindFriendsStorage5(context, f, frm);
                }
            }
        }
    }
}