﻿#pragma checksum "D:\Dropbox\Windows Phone\Sapper2 7.8 2.1.1.0\Sapper2\Views\NewCustomLevelPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "30267D084220985EA77E9FDF92BCC544"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
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
    
    
    public partial class NewCustomLevelPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox tbxLevelName;
        
        internal System.Windows.Controls.Slider sldrHorizontal;
        
        internal System.Windows.Controls.TextBox tbxHorizontal;
        
        internal System.Windows.Controls.Slider sldrVertical;
        
        internal System.Windows.Controls.TextBox tbxVertical;
        
        internal System.Windows.Controls.Slider sldrMineCount;
        
        internal System.Windows.Controls.TextBox tbxMineCount;
        
        internal System.Windows.Controls.Canvas Canvas_SelectedColor;
        
        internal System.Windows.Controls.TextBlock tblkHorizontalCount;
        
        internal System.Windows.Controls.TextBlock tblkVerticalCount;
        
        internal System.Windows.Controls.TextBlock tblkMinePercentage;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Sapper2;component/Views/NewCustomLevelPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.tbxLevelName = ((System.Windows.Controls.TextBox)(this.FindName("tbxLevelName")));
            this.sldrHorizontal = ((System.Windows.Controls.Slider)(this.FindName("sldrHorizontal")));
            this.tbxHorizontal = ((System.Windows.Controls.TextBox)(this.FindName("tbxHorizontal")));
            this.sldrVertical = ((System.Windows.Controls.Slider)(this.FindName("sldrVertical")));
            this.tbxVertical = ((System.Windows.Controls.TextBox)(this.FindName("tbxVertical")));
            this.sldrMineCount = ((System.Windows.Controls.Slider)(this.FindName("sldrMineCount")));
            this.tbxMineCount = ((System.Windows.Controls.TextBox)(this.FindName("tbxMineCount")));
            this.Canvas_SelectedColor = ((System.Windows.Controls.Canvas)(this.FindName("Canvas_SelectedColor")));
            this.tblkHorizontalCount = ((System.Windows.Controls.TextBlock)(this.FindName("tblkHorizontalCount")));
            this.tblkVerticalCount = ((System.Windows.Controls.TextBlock)(this.FindName("tblkVerticalCount")));
            this.tblkMinePercentage = ((System.Windows.Controls.TextBlock)(this.FindName("tblkMinePercentage")));
        }
    }
}

