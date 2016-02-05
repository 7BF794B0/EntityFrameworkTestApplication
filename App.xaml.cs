using System;
using System.Collections.Generic;
using Microsoft.Shell;

namespace EntityFrameworkTestApplication
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : ISingleInstanceApp
    {
        private const string Unique = "EntityFrameworkTestApplication";

        [STAThread]
        public static void Main()
        {
            if (!SingleInstance<App>.InitializeAsFirstInstance(Unique))
                return;

            var application = new App();
            application.InitializeComponent();
            application.Run(new Controller());

            // Allow single instance code to perform cleanup operations.
            SingleInstance<App>.Cleanup();
        }

        #region ISingleInstanceApp Members
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // Handle command line arguments of second instance.
            return true;
        }
        #endregion
    }
}