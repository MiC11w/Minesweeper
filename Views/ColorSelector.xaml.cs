using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.ComponentModel;
using System.Globalization;

namespace Sapper2.Views
{
    public partial class ColorSelector : PhoneApplicationPage, INotifyPropertyChanged
    {
        public ObservableCollection<SolidColorBrush> brushes { get; set; }

        public ColorSelector()
        {
            InitializeComponent();
            InitializeBrushes();
            lista.DataContext = this;
        }

        private void InitializeBrushes()
        {
            string[] colors = {     "FFA4C400", // LIME
                                    "FF60A917", // GREEN
                                    "FF008A00", // EMERLAND
                                    "FF00ABA9", // TEAL
                                    "FF1BA1E2", // CYAN
                                    "FF0050EF", // COBALT
                                    "FF6A00FF", // INDIGO
                                    "FFAA00FF", // VIOLET
                                    "FFF472D0", // PINK
                                    "FFD80073", // MAGNETA
                                    "FFA20025", // CRIMSON
                                    "FFE51400", // RED
                                    "FFFA6800", // ORANGE
                                    "FFF0A30A", // AMBER
                                    "FFD8C100", // YELLOW
                                    "FF825A2C", // BROWN
                                    "FF6D8764", // OLIVE
                                    "FF647687", // STEEL
                                    "FF76608A", // MAUVE
                                    "FF7A3B3F", // SIENNA
                              };

            brushes = new ObservableCollection<SolidColorBrush>();

            foreach (string hexCode in colors)
            {
                Color color = new Color();
                color.A = byte.Parse(hexCode.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                color.R = byte.Parse(hexCode.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                color.G = byte.Parse(hexCode.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                color.B = byte.Parse(hexCode.Substring(6, 2), NumberStyles.AllowHexSpecifier);


                SolidColorBrush brush = new SolidColorBrush(color);
                brushes.Add(brush);
            }            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Canvas_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            SolidColorBrush brush = canvas.DataContext as SolidColorBrush;
            App.ColorSelected = brush;
            NavigationService.GoBack();
        }
    }
}