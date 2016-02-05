using System.Windows.Controls;
using System.Windows.Media;
using EntityFrameworkTestApplication.Models;

namespace EntityFrameworkTestApplication.View
{
    /// <summary>
    /// Class to render changes on the form.
    /// </summary>
    class View
    {
        /// <summary>
        /// Display information about the state of Redis.
        /// </summary>
        /// <param name="redisIsStarted">The flag to check launched Redis.</param>
        /// <param name="lblRedis">Label where to display information.</param>
        public void RenderInformationAboutRedis(bool redisIsStarted, Label lblRedis)
        {
            if (redisIsStarted)
            {
                lblRedis.Content = "Redis-Server: It works!";
                lblRedis.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xB4, 0x00));
            }
            else
            {
                lblRedis.Content = "Redis-Server: Does not work!";
                lblRedis.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));
            }
        }

        /// <summary>
        /// Display information about the state of Sphinx.
        /// </summary>
        /// <param name="sphinxIsStarted">The flag to check launched Sphinx.</param>
        /// <param name="lblSphinx">Label where to display information.</param>
        /// <param name="grpSearch">GroupBox which must be unlocked.</param>
        /// <param name="txtSearchResult">TextBox which must be unlocked.</param>
        public void RenderInformationAboutSphinx(bool sphinxIsStarted, Label lblSphinx, GroupBox grpSearch, TextBox txtSearchResult)
        {
            if (sphinxIsStarted)
            {
                lblSphinx.Content = "Sphinx: It works!";
                lblSphinx.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xB4, 0x00));
            }
            else
            {
                lblSphinx.Content = "Sphinx: Does not work!";
                lblSphinx.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));
                grpSearch.IsEnabled = false;
                txtSearchResult.IsEnabled = false;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public void ChangeTextIntoTextBox(TextBox[] txtBoxs, User u)
        {
            txtBoxs[0].Text = u.First_Name;
            txtBoxs[1].Text = u.Last_Name;
            txtBoxs[2].Text = u.Age.ToString();
            txtBoxs[3].Text = u.Bio;
            txtBoxs[4].Text = u.Country;
            txtBoxs[5].Text = u.City;
        }
    }
}
