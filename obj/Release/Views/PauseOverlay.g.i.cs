﻿#pragma checksum "E:\Skydrive\Windows Phone\Sapper2 7.8\Sapper2\Views\PauseOverlay.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F6C425F3EB79D67BB2C40678789AACB1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Sapper2.Views {
    
    
    public partial class PauseOverlay : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button Button_Restart;
        
        internal System.Windows.Controls.Button Button_MainMenu;
        
        internal System.Windows.Controls.Button Button_Resume;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Sapper2;component/Views/PauseOverlay.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Button_Restart = ((System.Windows.Controls.Button)(this.FindName("Button_Restart")));
            this.Button_MainMenu = ((System.Windows.Controls.Button)(this.FindName("Button_MainMenu")));
            this.Button_Resume = ((System.Windows.Controls.Button)(this.FindName("Button_Resume")));
        }
    }
}

