using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Sapper2.Model;

namespace Sapper2
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DefaultLevelsPanoramaItem.DataContext = App.DefaultLevelsViewM;
            CustomLevelsPanoramaItem.DataContext = App.CustomLevelsViewM;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.DefaultLevelsViewM.IsDataLoaded)
            {
                App.DefaultLevelsViewM.LoadData();
            }
            if (!App.CustomLevelsViewM.IsDataLoaded)
            {
                App.CustomLevelsViewM.LoadData();
            }
        }

        private void EmptySlotContextCreateNew_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MenuItem clickedMenuItem = sender as MenuItem;
            var dataContext = clickedMenuItem.DataContext;
            //searching for listboxItem
            ListBoxItem clickedListBoxItem = ListBoxCustomLevels.ItemContainerGenerator.ContainerFromItem(dataContext) as ListBoxItem;
            //passing data to new page
            App.NaviData_SelectedLevelItem = clickedListBoxItem.DataContext as LevelItem;
            //navigating to new new page
            NavigationService.Navigate(new Uri("/Views/NewCustomLevelPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void UsedSlotContextEdit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MenuItem clickedMenuItem = sender as MenuItem;
            var dataContext = clickedMenuItem.DataContext;
            //searching for listboxItem
            ListBoxItem clickedListBoxItem = ListBoxCustomLevels.ItemContainerGenerator.ContainerFromItem(dataContext) as ListBoxItem;
            //passing data to new page
            App.NaviData_SelectedLevelItem = clickedListBoxItem.DataContext as LevelItem;
            //navigating to new new page
            NavigationService.Navigate(new Uri("/Views/NewCustomLevelPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void UsedSlotContextDelete_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MenuItem clickedMenuItem = sender as MenuItem;
            var dataContext = clickedMenuItem.DataContext;
            ListBoxItem clickedListBoxItem = ListBoxCustomLevels.ItemContainerGenerator.ContainerFromItem(dataContext) as ListBoxItem;
            LevelItem contextLevelItem = clickedListBoxItem.DataContext as LevelItem;

            contextLevelItem.Clear();
            App.CustomLevelsViewM.SubmitChanges();
        }

        private void EmptySlotBorderCreateNew_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel stack = sender as StackPanel;
            var dataContext = stack.DataContext;
            ListBoxItem clickedListBoxItem = ListBoxCustomLevels.ItemContainerGenerator.ContainerFromItem(dataContext) as ListBoxItem;
            //passing data to new page
            App.NaviData_SelectedLevelItem = clickedListBoxItem.DataContext as LevelItem;
            //disabling listbox selection
            clickedListBoxItem.IsSelected = false;
            //navigating to new new page
            NavigationService.Navigate(new Uri("/Views/NewCustomLevelPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void DefaultLevelsItemTemplate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.NaviData_SelectedLevelItem = (sender as StackPanel).DataContext as LevelItem;
            NavigationService.Navigate(new Uri("/Views/GamePage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void CustomLevelItemTemplate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.NaviData_SelectedLevelItem = (sender as StackPanel).DataContext as LevelItem;
            NavigationService.Navigate(new Uri("/Views/GamePage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void AppBar_About(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/About.xaml", UriKind.Relative));
        }


    }
}