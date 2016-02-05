using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using EntityFrameworkTestApplication.Model;
using EntityFrameworkTestApplication.Models;
using SphinxConnector.SphinxQL;

namespace EntityFrameworkTestApplication
{
    /// <summary>
    /// The main class of the application.
    /// Carries out all the work and interaction of the components.
    /// </summary>
    public class Controller : MainWindow
    {
        // A copy of the list lstFriends, but there are only stored id these people.
        private readonly List<int> _listOfFriends = new List<int>();
        // List of text boxes to fill.
        private readonly TextBox[] _textBoxs;

        private readonly Friend _friend = new Friend();
        private readonly Logging _logging = new Logging();
        private readonly Services _services = new Services();
        private readonly View.View _view = new View.View();

        public Controller()
        {
            _textBoxs = new[] { txtFirstName, txtLastName, txtAge, txtBio, txtCountry, txtCity };

            // Determine the state (ready) services.
            _services.GetServiceState(_logging);

            // Output status information on the form:
            _view.RenderInformationAboutRedis(_services.RedisIsStarted, lblRedis);
            _view.RenderInformationAboutSphinx(_services.SphinxIsStarted, lblSphinx, grpSearch, txtSearchResult);
        }

        /// <summary>
        /// Divides the input string by string on chunkSize (we have 1000) characters each, which would then add them in the "Bio".
        /// </summary>
        /// <param name="s">Line for processing.</param>
        /// <param name="chunkSize">The length of the new line.</param>
        /// <returns>ChunkSize - substring length.</returns>
        private static IEnumerable<string> TruncateLongString(string s, int chunkSize)
        {
            var chunkCount = s.Length / chunkSize;
            for (var i = 0; i < chunkCount; i++)
                yield return s.Substring(i * chunkSize, chunkSize);

            if (chunkSize * chunkCount < s.Length)
                yield return s.Substring(chunkSize * chunkCount);
        }

        /// <summary>
        /// Event: Complete database.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void btnFill_Click(object sender, RoutedEventArgs e)
        {
            #region The code required for diagnosis - measures the time
            var spectator = new Stopwatch();
            spectator.Start();
            #endregion
            var fd = new FillingData();
            int i, bl;

            var name = File.ReadAllLines("name.txt");
            var surname = File.ReadAllLines("surname.txt");
            var book1 = TruncateLongString(string.Join(" ", File.ReadAllLines("book-1.txt")), 1000).ToArray();
            var book2 = TruncateLongString(string.Join(" ", File.ReadAllLines("book-2.txt")), 1000).ToArray();

            var rnd = new Random();

            var usersForStorage1 = new List<User>();
            var usersForStorage2 = new List<User>();
            var usersForStorage3 = new List<User>();
            var usersForStorage4 = new List<User>();
            var usersForStorage5 = new List<User>();

            for (i = 0; i < book1.Length; i++)
            {
                var user = new User
                {
                    Id = i + 1,
                    First_Name = name[rnd.Next(name.Length)],
                    Last_Name = surname[rnd.Next(surname.Length)],
                    Age = rnd.Next(14, 80),
                    Country = "Russia",
                    City = (rnd.Next(0, 2) == 0) ? "St. Petersburg" : "Moscow",
                    Bio = book1[i]
                };
                usersForStorage1.Add(user);
            }

            for (bl = 0; i < 100000; i++, bl++)
            {
                if (!(bl < book2.Length)) bl = 0;
                var user = new User { Id = i + 1, First_Name = name[rnd.Next(name.Length)], Last_Name = surname[rnd.Next(surname.Length)], Age = rnd.Next(14, 80), Country = "Russia", City = (rnd.Next(0, 2) == 0) ? "St. Petersburg" : "Moscow", Bio = book2[bl] };
                usersForStorage1.Add(user);
            }

            for (i = 100000; i < 200000; i++, bl++)
            {
                if (!(bl < book2.Length)) bl = 0;
                var user = new User { Id = i + 1, First_Name = name[rnd.Next(name.Length)], Last_Name = surname[rnd.Next(surname.Length)], Age = rnd.Next(14, 80), Country = "Russia", City = (rnd.Next(0, 2) == 0) ? "St. Petersburg" : "Moscow", Bio = book2[bl] };
                usersForStorage2.Add(user);
            }

            for (i = 200000; i < 300000; i++, bl++)
            {
                if (!(bl < book2.Length)) bl = 0;
                var user = new User { Id = i + 1, First_Name = name[rnd.Next(name.Length)], Last_Name = surname[rnd.Next(surname.Length)], Age = rnd.Next(14, 80), Country = "Russia", City = (rnd.Next(0, 2) == 0) ? "St. Petersburg" : "Moscow", Bio = book2[bl] };
                usersForStorage3.Add(user);
            }

            for (i = 300000; i < 400000; i++, bl++)
            {
                if (!(bl < book2.Length)) bl = 0;
                var user = new User { Id = i + 1, First_Name = name[rnd.Next(name.Length)], Last_Name = surname[rnd.Next(surname.Length)], Age = rnd.Next(14, 80), Country = "Russia", City = (rnd.Next(0, 2) == 0) ? "St. Petersburg" : "Moscow", Bio = book2[bl] };
                usersForStorage4.Add(user);
            }

            for (i = 400000; i < 500000; i++, bl++)
            {
                if (!(bl < book2.Length)) bl = 0;
                var user = new User { Id = i + 1, First_Name = name[rnd.Next(name.Length)], Last_Name = surname[rnd.Next(surname.Length)], Age = rnd.Next(14, 80), Country = "Russia", City = (rnd.Next(0, 2) == 0) ? "St. Petersburg" : "Moscow", Bio = book2[bl] };
                usersForStorage5.Add(user);
            }

            var transactionScopeWithStorage1 = new Task(() => fd.TransactionScopeWithStorage1(usersForStorage1));
            var transactionScopeWithStorage2 = new Task(() => fd.TransactionScopeWithStorage2(usersForStorage2));
            var transactionScopeWithStorage3 = new Task(() => fd.TransactionScopeWithStorage3(usersForStorage3));
            var transactionScopeWithStorage4 = new Task(() => fd.TransactionScopeWithStorage4(usersForStorage4));
            var transactionScopeWithStorage5 = new Task(() => fd.TransactionScopeWithStorage5(usersForStorage5));

            transactionScopeWithStorage1.Start();
            transactionScopeWithStorage2.Start();
            transactionScopeWithStorage3.Start();
            transactionScopeWithStorage4.Start();
            transactionScopeWithStorage5.Start();

            Task.WaitAll(transactionScopeWithStorage1, transactionScopeWithStorage2, transactionScopeWithStorage3, transactionScopeWithStorage4, transactionScopeWithStorage5);
            #region The code required for diagnosis - measures the time
            spectator.Stop();
            var ts = spectator.Elapsed;
            lblTime.Content = $"Lead time: {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds/10:00}";
            #endregion
        }

        /// <summary>
        /// Determine the database with which it is necessary to make the connection.
        /// </summary>
        /// <param name="userId">User ID.</param>
        private void Connected(int userId)
        {
            try
            {
                var localDb = new DataBase();

                if (userId <= 100000)
                {
                    using (var context = new Storage1Context())
                    {
                        localDb.ConnectToServer1(context, userId, _listOfFriends, _services, _logging, _friend, _view, _textBoxs, lstFriends);
                    }
                }
                else if (userId > 100000 && userId <= 200000)
                {
                    using (var context = new Storage2Context())
                    {
                        localDb.ConnectToServer2(context, userId, _listOfFriends, _services, _logging, _friend, _view, _textBoxs, lstFriends);
                    }
                }
                else if (userId > 200000 && userId <= 300000)
                {
                    using (var context = new Storage3Context())
                    {
                        localDb.ConnectToServer3(context, userId, _listOfFriends, _services, _logging, _friend, _view, _textBoxs, lstFriends);
                    }
                }
                else if (userId > 300000 && userId <= 400000)
                {
                    using (var context = new Storage4Context())
                    {
                        localDb.ConnectToServer4(context, userId, _listOfFriends, _services, _logging, _friend, _view, _textBoxs, lstFriends);
                    }
                }
                else if (userId > 400000 && userId <= 500000)
                {
                    using (var context = new Storage5Context())
                    {
                        localDb.ConnectToServer5(context, userId, _listOfFriends, _services, _logging, _friend, _view, _textBoxs, lstFriends);
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logging.ProcessingException(ex);
                MessageBox.Show(ex.Message, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Determine the database with which it is necessary to make the connection.
        /// The method for the modal form.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="frm">An instance of a modal form.</param>
        private void Connected(int userId, InfoWindow frm)
        {
            try
            {
                var localDb = new DataBase();

                if (userId <= 100000)
                {
                    using (var context = new Storage1Context())
                    {
                        localDb.ConnectToServer1(context, userId, _services, _logging, _friend, lstFriends, frm);
                    }
                }
                else if (userId > 100000 && userId <= 200000)
                {
                    using (var context = new Storage2Context())
                    {
                        localDb.ConnectToServer2(context, userId, _services, _logging, _friend, lstFriends, frm);
                    }
                }
                else if (userId > 200000 && userId <= 300000)
                {
                    using (var context = new Storage3Context())
                    {
                        localDb.ConnectToServer3(context, userId, _services, _logging, _friend, lstFriends, frm);
                    }
                }
                else if (userId > 300000 && userId <= 400000)
                {
                    using (var context = new Storage4Context())
                    {
                        localDb.ConnectToServer4(context, userId, _services, _logging, _friend, lstFriends, frm);
                    }
                }
                else if (userId > 400000 && userId <= 500000)
                {
                    using (var context = new Storage5Context())
                    {
                        localDb.ConnectToServer5(context, userId, _services, _logging, _friend, lstFriends, frm);
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logging.ProcessingException(ex);
                MessageBox.Show(ex.Message, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// The calculation of the MD5 password.
        /// </summary>
        /// <param name="passwordToHash">Password.</param>
        /// <returns>MD5-Hash of the password.</returns>
        private static string GetMd5Hash(string passwordToHash)
        {
            if ((passwordToHash == null) || (passwordToHash.Length == 0)) return string.Empty;

            // Need MD5 to calculate the hash.
            // Byte array representation of that string.
            var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(new UTF8Encoding().GetBytes(passwordToHash));

            // String representation (similar to UNIX format) without dashes in lowercase.
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Event: Connect to the database.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void btnConnected_Click(object sender, RoutedEventArgs e)
        {
            #region The code required for diagnosis - measures the time
            var spectator = new Stopwatch();
            spectator.Start();
            #endregion
            // Sample.
            lstFriends.Items.Clear();
            _listOfFriends.Clear();

            using (var mapContext = new MapContext())
            {
                IQueryable<Map> queryMap = null;

                if (txtLogin.Text == string.Empty && txtPassword.Text == string.Empty && txtId.Text == string.Empty)
                    MessageBox.Show("More than one field is not filled", "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (txtLogin.Text == string.Empty && txtPassword.Text == string.Empty && Convert.ToInt32(txtId.Text) < 500001)
                {
                    var idFromTxt = Convert.ToInt32(txtId.Text);
                    queryMap = mapContext.Map.Where(x => x.User_Id == idFromTxt);
                }
                else if (txtLogin.Text != string.Empty && txtPassword.Text != string.Empty && txtId.Text == string.Empty)
                {
                    var pass = GetMd5Hash(txtPassword.Text);
                    queryMap = mapContext.Map.Where(x => x.Login == txtLogin.Text.ToString() && x.Password == pass);
                }
                else
                    MessageBox.Show("Check the correctness of filling the fields", "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);

                if (queryMap != null)
                {
                    var find = queryMap.ToList();
                    var userId = find[0].User_Id;
                    Connected(userId);
                }
            }
            #region The code required for diagnosis - measures the time
            spectator.Stop();
            var ts = spectator.Elapsed;
            lblTime.Content = $"Lead time: {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds/10:00}";
            #endregion
        }

        /// <summary>
        /// Event: Make all friends.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void btnFriends_Click(object sender, RoutedEventArgs e)
        {
            var arrayNumberFriends = new int[500000];

            for (var i = 0; i < 500000; i++) arrayNumberFriends[i] = 0;

            var rnd = new Random();

            var storage1Context = new Storage1Context();
            var storage2Context = new Storage2Context();
            var storage3Context = new Storage3Context();
            var storage4Context = new Storage4Context();
            var storage5Context = new Storage5Context();

            storage1Context.Configuration.AutoDetectChangesEnabled = false;
            storage2Context.Configuration.AutoDetectChangesEnabled = false;
            storage3Context.Configuration.AutoDetectChangesEnabled = false;
            storage4Context.Configuration.AutoDetectChangesEnabled = false;
            storage5Context.Configuration.AutoDetectChangesEnabled = false;

            for (var i = 0; i < 500000; i++)
            {
                if (arrayNumberFriends[i] == 5)
                    continue;

                var shortage = 5 - arrayNumberFriends[i];
                for (var j = 0; j < shortage; j++)
                {
                    var friendId = rnd.Next(1, 500001);
                    if (i == friendId)
                        friendId = rnd.Next(1, 500001);

                    if (i <= 100000)
                    {
                        storage1Context.Friends.Add(new Friends { User_Id = i + 1, Friend_Id = friendId });
                        storage1Context.SaveChanges();
                        arrayNumberFriends[i]++;
                    }
                    else if (i > 100000 && i < 200000)
                    {
                        storage2Context.Friends.Add(new Friends { User_Id = i, Friend_Id = friendId });
                        storage2Context.SaveChanges();
                        arrayNumberFriends[i]++;
                    }
                    else if (i > 200000 && i < 300000)
                    {
                        storage3Context.Friends.Add(new Friends { User_Id = i, Friend_Id = friendId });
                        storage3Context.SaveChanges();
                        arrayNumberFriends[i]++;
                    }
                    else if (i > 300000 && i < 400000)
                    {
                        storage4Context.Friends.Add(new Friends { User_Id = i, Friend_Id = friendId });
                        storage4Context.SaveChanges();
                        arrayNumberFriends[i]++;
                    }
                    else if (i > 400000 && i < 500000)
                    {
                        storage5Context.Friends.Add(new Friends { User_Id = i, Friend_Id = friendId });
                        storage5Context.SaveChanges();
                        arrayNumberFriends[i]++;
                    }

                    if (friendId > 1 && friendId < 100000)
                    {
                        storage1Context.Friends.Add(new Friends { User_Id = friendId, Friend_Id = (i <= 100000) ? i + 1 : i });
                        storage1Context.SaveChanges();
                        arrayNumberFriends[friendId]++;
                    }
                    else if (friendId > 100000 && friendId < 200000)
                    {
                        storage2Context.Friends.Add(new Friends { User_Id = friendId, Friend_Id = (i <= 100000) ? i + 1 : i });
                        storage2Context.SaveChanges();
                        arrayNumberFriends[friendId]++;
                    }
                    else if (friendId > 200000 && friendId < 300000)
                    {
                        storage3Context.Friends.Add(new Friends { User_Id = friendId, Friend_Id = (i <= 100000) ? i + 1 : i });
                        storage3Context.SaveChanges();
                        arrayNumberFriends[friendId]++;
                    }
                    else if (friendId > 300000 && friendId < 400000)
                    {
                        storage4Context.Friends.Add(new Friends { User_Id = friendId, Friend_Id = (i <= 100000) ? i + 1 : i });
                        storage4Context.SaveChanges();
                        arrayNumberFriends[friendId]++;
                    }
                    else if (friendId > 400000 && friendId < 500000)
                    {
                        storage5Context.Friends.Add(new Friends { User_Id = friendId, Friend_Id = (i <= 100000) ? i + 1 : i });
                        storage5Context.SaveChanges();
                        arrayNumberFriends[friendId]++;
                    }
                }
            }
        }

        /// <summary>
        /// Event: Get detailed information about a friend.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void lstFriends_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            #region The code required for diagnosis - measures the time
            var spectator = new Stopwatch();
            spectator.Start();
            #endregion

            if (lstFriends.SelectedItem == null)
                return;

            var frm = new InfoWindow
            {
                StartedRedis = _services.RedisIsStarted,
                // Do not show on the taskbar.
                ShowInTaskbar = false
            };

            Connected(_listOfFriends[lstFriends.SelectedIndex], frm);
            #region The code required for diagnosis - measures the time
            spectator.Stop();
            var ts = spectator.Elapsed;
            frm.TimeParam = $"Lead time: {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds/10:00}";
            #endregion
            frm.Show();
        }

        /// <summary>
        /// Event: information search (using a search form).
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearchResult.Text = string.Empty;
            var ds = new DataSet();
            try
            {
                using (var connection = new SphinxQLConnection(@"Data Source=localhost;Port=9306"))
                {
                    var selectCommand = new SphinxQLCommand(connection);
                    // We shape our query string.
                    var commandText = new StringBuilder();

                    if (txtSearchFirstName.Text != string.Empty)
                    {
                        selectCommand.Parameters.Add("@matchFirstName", txtSearchFirstName.Text);
                        commandText.Append(@"SELECT * FROM indexstore1, indexstore2, indexstore3, indexstore4, indexstore5 WHERE MATCH(@matchFirstName);");
                    }

                    if (txtSearchLastName.Text != string.Empty)
                    {
                        selectCommand.Parameters.Add("@matchLastName", txtSearchLastName.Text);
                        commandText.Append(@"SELECT * FROM indexstore1, indexstore2, indexstore3, indexstore4, indexstore5 WHERE MATCH(@matchLastName);");
                    }

                    if (txtSearchAge.Text != string.Empty)
                    {
                        switch (cmbSearchAge.SelectedIndex)
                        {
                            case 0:
                                commandText.Append(
                                    $@"SELECT * FROM indexstore1, indexstore2, indexstore3, indexstore4, indexstore5 WHERE AGE > {txtSearchAge.Text};");
                                break;
                            case 1:
                                commandText.Append(
                                    $@"SELECT * FROM indexstore1, indexstore2, indexstore3, indexstore4, indexstore5 WHERE AGE < {txtSearchAge.Text};");
                                break;
                            case 2:
                                commandText.Append(
                                    $@"SELECT * FROM indexstore1, indexstore2, indexstore3, indexstore4, indexstore5 WHERE AGE = {txtSearchAge.Text};");
                                break;
                        }
                    }

                    if (txtSearchCountry.Text != string.Empty)
                    {
                        selectCommand.Parameters.Add("@matchCountry", txtSearchCountry.Text);
                        commandText.Append(@"SELECT * FROM indexstore1, indexstore2, indexstore3, indexstore4, indexstore5 WHERE MATCH(@matchCountry);");
                    }

                    if (txtSearchCity.Text != string.Empty)
                    {
                        selectCommand.Parameters.Add("@matchCity", txtSearchCity.Text);
                        commandText.Append(@"SELECT * FROM indexstore1, indexstore2, indexstore3, indexstore4, indexstore5 WHERE MATCH(@matchCity);");
                    }

                    if (txtSearchBio.Text != string.Empty)
                    {
                        selectCommand.Parameters.Add("@matchBio", txtSearchBio.Text);
                        commandText.Append(@"SELECT * FROM indexstore1, indexstore2, indexstore3, indexstore4, indexstore5 WHERE MATCH(@matchBio);");
                    }

                    selectCommand.CommandText = commandText.ToString().Substring(0, commandText.Length - 1);
                    var dataAdapter = new SphinxQLDataAdapter {SelectCommand = selectCommand};
                    dataAdapter.Fill(ds);
                }
            }
            catch (SphinxQLException ex)
            {
                _logging.ProcessingException(ex);
            }

            // Parsing XML.
            using (var reader = XmlReader.Create(new StringReader(ds.GetXml())))
            {
                while (reader.Read())
                {
                    if (!reader.IsStartElement())
                        continue;
                    switch (reader.Name)
                    {
                        case "NewDataSet": break;
                        case "Table": break;
                        case "id": break;
                        case "first_name":
                            if (reader.Read())
                                txtSearchResult.Text += $"First Name: {reader.Value.Trim()}\n";
                            break;
                        case "last_name":
                            if (reader.Read())
                                txtSearchResult.Text += $"Second Name: {reader.Value.Trim()}\n";
                            break;
                        case "age":
                            if (reader.Read())
                                txtSearchResult.Text += $"Age: {reader.Value.Trim()}\n";
                            break;
                        case "bio":
                            if (reader.Read())
                                txtSearchResult.Text += $"Bio: {reader.Value.Trim()}\n";
                            break;
                        case "country":
                            if (reader.Read())
                                txtSearchResult.Text += $"Country: {reader.Value.Trim()}\n";
                            break;
                        case "city":
                            if (reader.Read())
                                txtSearchResult.Text += $"City: {reader.Value.Trim()}\n\n";
                            break;
                    }
                }
            }
        }
    }
}