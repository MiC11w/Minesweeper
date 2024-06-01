using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Sapper2.Views
{
    public partial class VictoryOverlay : PhoneApplicationPage
    {
        public event EventHandler OnButtonMainMenu_Tap;
        public event EventHandler OnButtonRestart_Tap;

        public VictoryOverlay()
        {
            InitializeComponent();
            Button_Restart.Opacity = 0;
            Button_MainMenu.Opacity = 0;
        }

        public void SetTimes(TimeSpan bestTime, TimeSpan yourTime, bool highscoreExist)
        {
            if (highscoreExist == false) 
                this.TextBlock_BestTime.Text = "--:--";
            else 
                this.TextBlock_BestTime.Text = bestTime.ToString(@"mm\:ss");

            this.TextBlock_YourTime.Text = yourTime.ToString(@"mm\:ss");

            if (bestTime > yourTime || highscoreExist == false)
            {
                BestTimeStoryboard.Completed += BestTimeStoryboard_Completed;
                BestTimeStoryboard.Begin();
            }
            else
            {
                Grid_BestTimeMessage.Visibility = System.Windows.Visibility.Collapsed;
                ButtonsStoryboard.Begin();
            }
        }

        void BestTimeStoryboard_Completed(object sender, EventArgs e)
        {
            ButtonsStoryboard.Begin();
        }

        private void Button_MainMenu_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            OnButtonMainMenu_Tap(this, new EventArgs());
        }

        private void Button_Restart_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            OnButtonRestart_Tap(this, new EventArgs());
        }
    }
}