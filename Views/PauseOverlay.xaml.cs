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
    public partial class PauseOverlay : PhoneApplicationPage
    {
        public event EventHandler OnButtonResume_Tap;
        public event EventHandler OnButtonMainMenu_Tap;
        public event EventHandler OnButtonRestart_Tap;

        public PauseOverlay()
        {
            InitializeComponent();
        }

        private void Button_Resume_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            OnButtonResume_Tap(this, new EventArgs());
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