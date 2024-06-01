using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Sapper2.ViewModels;
using Sapper2.Model;
using Sapper2.Resources;
using System.Reflection;

namespace Sapper2
{
    public partial class App : Application
    {
        private static LevelsViewModel defaultLevelsViewModel = null;
        private static LevelsViewModel customLevelsViewModel = null;

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        public static LevelsViewModel DefaultLevelsViewM
        {
            get
            {
                return defaultLevelsViewModel;
            }
        }

        public static string GetVersion()
        {
            var versionAttribute = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true).FirstOrDefault() as AssemblyFileVersionAttribute;

            if (versionAttribute != null)
            {
                return versionAttribute.Version;
            }
            return "";
        }

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        public static LevelsViewModel CustomLevelsViewM
        {
            get
            {
                return customLevelsViewModel;
            }
        }

        public static LevelItem NaviData_SelectedLevelItem { get; set; }
        public static LevelsViewModel NaviData_SelectedViewModel { get; set; }
        public static SolidColorBrush ColorSelected { get; set; }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            // Specify the local database connection string.
            string DefaultLevelsDBConnectionString = "Data Source=isostore:/DefaultLevels.sdf";
            string CustomLevelsDBConnectionString = "Data Source=isostore:/CustomLevels.sdf";

            // Create the default levels database if it does not exist.
            using (LevelsDataContext DefaultLevelsDB = new LevelsDataContext(DefaultLevelsDBConnectionString))
            {
                if (DefaultLevelsDB.DatabaseExists() == false)
                {
                    // Create the local database.
                    DefaultLevelsDB.CreateDatabase();

                    // Prepopulate the categories.
                    DefaultLevelsDB.Items.InsertOnSubmit(new LevelItem { LevelNameItem = AppResources.Level_Easy, BoardXItem = 8, BoardYItem = 11, MinesNumberItem = 12 });
                    DefaultLevelsDB.Items.InsertOnSubmit(new LevelItem { LevelNameItem = AppResources.Level_Medium, BoardXItem = 8, BoardYItem = 18, MinesNumberItem = 20 });
                    DefaultLevelsDB.Items.InsertOnSubmit(new LevelItem { LevelNameItem = AppResources.Level_Hard, BoardXItem = 12, BoardYItem = 16, MinesNumberItem = 30 });
                    DefaultLevelsDB.Items.InsertOnSubmit(new LevelItem { LevelNameItem = AppResources.Level_VeryHard, BoardXItem = 14, BoardYItem = 18, MinesNumberItem = 44 });
                    DefaultLevelsDB.Items.InsertOnSubmit(new LevelItem { LevelNameItem = AppResources.Level_Brutal, BoardXItem = 16, BoardYItem = 20, MinesNumberItem = 62 });

                    // Save Items to the database.
                    DefaultLevelsDB.SubmitChanges();
                }
            }
            using (LevelsDataContext CustomLevelsDB = new LevelsDataContext(CustomLevelsDBConnectionString))
            {
                if (CustomLevelsDB.DatabaseExists() == false || CustomLevelsDB.Items.Count<LevelItem>() != 6)
                {
                    CustomLevelsDB.DeleteDatabase();
                    // Create the local database.
                    CustomLevelsDB.CreateDatabase();
                    for (int i = 0; i < 6; i++)
                    {
                        CustomLevelsDB.Items.InsertOnSubmit(new LevelItem());
                    }

                    // Save Items to the database.
                    CustomLevelsDB.SubmitChanges();
                }
            }

            // Create the ViewModel object.
            defaultLevelsViewModel = new LevelsViewModel(DefaultLevelsDBConnectionString);
            defaultLevelsViewModel.LoadData();
            customLevelsViewModel = new LevelsViewModel(CustomLevelsDBConnectionString);
            customLevelsViewModel.LoadData();

            ColorSelected = this.Resources["PhoneAccentBrush"] as SolidColorBrush;
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Ensure that application state is restored appropriately
            if (!App.DefaultLevelsViewM.IsDataLoaded)
            {
                App.DefaultLevelsViewM.LoadData();
            }
            if (!App.CustomLevelsViewM.IsDataLoaded)
            {
                App.CustomLevelsViewM.LoadData();
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}