using System.Windows;
using System.Windows.Input;

namespace EntityFrameworkTestApplication
{
    /// <summary>
    /// The logic for the interaction MainWindow.xaml
    /// </summary>
    abstract public partial class MainWindow
    {
        protected MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event: Complete database.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        virtual protected void btnFill_Click(object sender, RoutedEventArgs e) { }

        /// <summary>
        /// Event: Connect to the database.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        virtual protected void btnConnected_Click(object sender, RoutedEventArgs e) { }

        /// <summary>
        /// Event: Make all friends.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        virtual protected void btnFriends_Click(object sender, RoutedEventArgs e) { }

        /// <summary>
        /// Event: Get detailed information about a friend.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        virtual protected void lstFriends_MouseDoubleClick(object sender, MouseButtonEventArgs e) { }

        /// <summary>
        /// Event: Information search (using a search form).
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        virtual protected void btnSearch_Click(object sender, RoutedEventArgs e) { }
    }
}