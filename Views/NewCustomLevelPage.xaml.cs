using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Sapper2.Model;
using System.Windows.Controls.Primitives;
using Sapper2.ViewModels;
using System.Windows.Media;

namespace Sapper2.Views
{
    public partial class NewCustomLevelPage : PhoneApplicationPage
    {
        public NewCustomLevelPage()
        {
            InitializeComponent();

            levelItemToEdit = App.NaviData_SelectedLevelItem;
            this.tbxLevelName.Text = levelItemToEdit.LevelNameItem;
            this.sldrHorizontal.Value = levelItemToEdit.BoardXItem;
            this.sldrVertical.Value = levelItemToEdit.BoardYItem;
            this.sldrMineCount.Value = levelItemToEdit.MinesNumberItem;
            this.Canvas_SelectedColor.Background = levelItemToEdit.ColorBrush;
        }

        private LevelItem levelItemToEdit { get; set; }

        private void ApplicationBarSave_Click(object sender, EventArgs e)
        {
            levelItemToEdit.LevelNameItem = tbxLevelName.Text;
            levelItemToEdit.BoardXItem = (int)sldrHorizontal.Value;
            levelItemToEdit.BoardYItem = (int)sldrVertical.Value;
            levelItemToEdit.MinesNumberItem = (int)sldrMineCount.Value;
            levelItemToEdit.ColorBrush = Canvas_SelectedColor.Background as SolidColorBrush;
            levelItemToEdit.ClearTime();

            App.CustomLevelsViewM.SubmitChanges();

            NavigationService.GoBack();
        }

        private void ApplicationBarCancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void sldrHorizontal_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbxHorizontal == null) return;
            tbxHorizontal.Text = ((int)(sender as Slider).Value).ToString();
            sldrMineCount.Maximum = (int)(sldrHorizontal.Value * sldrVertical.Value * 0.3);
            SummaryUpdate();
        }


        private void sldrVertical_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbxVertical == null) return;
            tbxVertical.Text = ((int)(sender as Slider).Value).ToString();
            sldrMineCount.Maximum = (int)(sldrHorizontal.Value * sldrVertical.Value * 0.3);
            SummaryUpdate();
        }

        private void tbxVertical_LostFocus(object sender, RoutedEventArgs e)
        {
            tbxVertical.Text = sldrVertical.Value.ToString();
        }

        private void tbxHorizontal_LostFocus(object sender, RoutedEventArgs e)
        {
            tbxHorizontal.Text = sldrHorizontal.Value.ToString();
        }

        private void sldrMineCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbxMineCount == null) return;
            tbxMineCount.Text = ((int)(sender as Slider).Value).ToString();
            SummaryUpdate();
        }

        private void SummaryUpdate()
        {
            tblkHorizontalCount.Text = tbxHorizontal.Text;
            tblkVerticalCount.Text = tbxVertical.Text;
            double boardRes = (int)sldrVertical.Value * (int)sldrHorizontal.Value;
            double minePercentage = ((int)sldrMineCount.Value / boardRes) * 100d;
            tblkMinePercentage.Text = Math.Round(minePercentage, 1).ToString();
        }

        private void tbxMineCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            int mineCount;
            try
            {
                mineCount = int.Parse(tbxMineCount.Text);
            }
            catch
            {
                sldrMineCount.Value = 1;
                return;
            }

            sldrMineCount.Value = mineCount;
            tbxMineCount.Text = sldrMineCount.Value.ToString();
        }

        private void Canvas_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ColorSelector.xaml", UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.ColorSelected != null || Canvas_SelectedColor.Background != null)
            {
                Canvas_SelectedColor.Background = App.ColorSelected;
            }
        }

    }
}