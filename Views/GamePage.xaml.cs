using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Sapper2.GameLogic;
using Sapper2.Model;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using Sapper2.Resources;

namespace Sapper2.Views
{
    public partial class GamePage : PhoneApplicationPage
    {
        private Popup _overlayPopup;
        private bool _popupState;

        SapperGame Sapper { get; set; }
        LevelItem CurrentLevelItem { get; set; }
        Storyboard anim;

        public GamePage()
        {
            InitializeComponent();
            CurrentLevelItem = App.NaviData_SelectedLevelItem;
            anim = stackPanel_FlagCount.Resources["FlagAnim"] as Storyboard;
            Storyboard.SetTarget(anim, stackPanel_FlagCount);

            _popupState = true;
            _overlayPopup = new Popup();
            ShowLoadingOverlay();

            //creating sapper asynchronously (board grid rendering takes time)
            Dispatcher.BeginInvoke(() =>
            {
                Sapper = new SapperGame(this, this.GridGameBoard, CurrentLevelItem.BoardXItem, CurrentLevelItem.BoardYItem, CurrentLevelItem.MinesNumberItem, CurrentLevelItem.ColorBrush);
                Sapper.Tick += OnTimeTickTextblockUpdate;
                Sapper.FlagNumberChanged += Sapper_OnFlagCountChanged;
                Sapper.RevealedSquaresNumberChanged += Sapper_OnRevealedSquaresCountChanged;
                Sapper.GameIsOver += Sapper_OnGameOver;
                Sapper.GameWin += Sapper_OnWin;                

                Sapper_OnFlagCountChanged(this, new EventArgs());
                Sapper_OnRevealedSquaresCountChanged(this, new EventArgs());

                HideLoadingOverlay();
            });

            ShowSapperAppBar();
        }

        void Sapper_OnWin(object sender, EventArgs e)
        {
            TimeSpan bestTime = TimeSpan.FromSeconds(CurrentLevelItem.BestTimeItem);
            TimeSpan yourTime = TimeSpan.FromSeconds(Sapper.GameTime);

            ShowVictoryOverlay(bestTime, yourTime, CurrentLevelItem.BestTimeExistsItem);

            if (CurrentLevelItem.BestTimeExistsItem == false || bestTime.TotalSeconds > yourTime.TotalSeconds)
            {
                SaveBestTime();
            }
        }

        void Sapper_OnGameOver(object sender, EventArgs e)
        {
            ShowGameOverOverlay();
        }

        void Sapper_OnRevealedSquaresCountChanged(object sender, EventArgs e)
        {
            double fraction = (double)(Sapper.RevealedSquaresNumber + Sapper.FlagNumber) / (CurrentLevelItem.BoardXItem * CurrentLevelItem.BoardYItem);
            tblRevealed.Text = ((int)(fraction * 100)).ToString();
        }

        void Sapper_OnFlagCountChanged(object sender, EventArgs e)
        {
            tblFlagi.Text = Sapper.FlagNumber.ToString();
            tblMiny.Text = CurrentLevelItem.MinesNumberItem.ToString();
            Sapper_OnRevealedSquaresCountChanged(this, new EventArgs());

            anim.Stop();
            anim.Begin();
        }

        public void OnTimeTickTextblockUpdate(object sender, EventArgs e)
        {
            tblGameTime.Text = TimeSpan.FromSeconds(Sapper.GameTime).ToString(@"mm\:ss");
        }



        private void SaveBestTime()
        {
            CurrentLevelItem.BestTimeItem = Sapper.GameTime;
            if (App.CustomLevelsViewM.LevelItems.Contains(CurrentLevelItem))
            {
                App.CustomLevelsViewM.SubmitChanges();
            }
            else
            {
                App.DefaultLevelsViewM.SubmitChanges();
            }
        }

        #region Overrided

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (Sapper.State == SapperGame.SapperState.GameOn) ShowPauseMenuOverlay();

            _popupState = _overlayPopup.IsOpen;
            _overlayPopup.IsOpen = false;

            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _overlayPopup.IsOpen = _popupState;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (Sapper.State == SapperGame.SapperState.GameOn)
            {
                ShowPauseMenuOverlay();
                e.Cancel = true;
            }
            base.OnBackKeyPress(e);
        }

        #endregion

        #region Overlays

        // Pause

        private void ShowPauseMenuOverlay()
        {
            //creating popup
            PauseOverlay pauseOverlay = new PauseOverlay();
            pauseOverlay.OnButtonResume_Tap += OnPauseOverlayResume_Tap;
            pauseOverlay.OnButtonMainMenu_Tap += OnPauseOverlayMainMenu_Tap;
            pauseOverlay.OnButtonRestart_Tap += pauseOverlay_OnButtonRestart_Tap;
            _overlayPopup.Child = pauseOverlay;

            Sapper.Pause();
            this.LayoutRoot.Opacity = 0.2;
            this.LayoutRoot.IsHitTestVisible = false;
            this.ApplicationBar.IsVisible = false;
            _overlayPopup.IsOpen = true;
            ShowSapperAppBar();
        }

        public void OnPauseOverlayResume_Tap(object sender, EventArgs e)
        {
            HidePauseMenuOverlay();
            Sapper.Resume();
        }

        void pauseOverlay_OnButtonRestart_Tap(object sender, EventArgs e)
        {
            Sapper.Restart();
            OnTimeTickTextblockUpdate(this, new EventArgs());
            Sapper_OnFlagCountChanged(this, new EventArgs());
            HidePauseMenuOverlay();
        }

        public void OnPauseOverlayMainMenu_Tap(object sender, EventArgs e)
        {
            HidePauseMenuOverlay();
            NavigationService.GoBack();
        }

        private void HidePauseMenuOverlay()
        {
            if (_overlayPopup.Child is PauseOverlay == false) throw new Exception("wrong overlay");

            this.LayoutRoot.Opacity = 1;
            this.LayoutRoot.IsHitTestVisible = true;
            this.ApplicationBar.IsVisible = true;
            _overlayPopup.IsOpen = false;
        }

        // Loading

        private void ShowLoadingOverlay()
        {
            //creating popup
            _overlayPopup.Child = new LoadingOverlay();

            this.LayoutRoot.Opacity = 0.2;
            _overlayPopup.IsOpen = true;
        }

        private void HideLoadingOverlay()
        {
            if (_overlayPopup.Child is LoadingOverlay == false) throw new Exception("wrong overlay");

            this.LayoutRoot.Opacity = 1.0;
            _overlayPopup.IsOpen = false;
        }

        // Game Over

        private void ShowGameOverOverlay()
        {
            //creating popup
            _overlayPopup.Child = new GameOverOverlay();

            _overlayPopup.IsOpen = true;
            ShowGameOverAppBar();
        }

        private void HideGameOverOverlay()
        {
            if (_overlayPopup.Child is GameOverOverlay == false) throw new Exception("wrong overlay");

            _overlayPopup.IsOpen = false;
            ShowSapperAppBar();
        }

        // Victory

        private void ShowVictoryOverlay(TimeSpan bestTime, TimeSpan yourTime, bool levelItemUntouched)
        {
            VictoryOverlay victoryOverlay = new VictoryOverlay();
            victoryOverlay.OnButtonMainMenu_Tap += victoryOverlay_OnButtonMainMenu_Tap;
            victoryOverlay.OnButtonRestart_Tap += victoryOverlay_OnButtonRestart_Tap;
            victoryOverlay.SetTimes(bestTime, yourTime, levelItemUntouched);
            _overlayPopup.Child = victoryOverlay;

            this.LayoutRoot.Opacity = 0.2;
            this.ApplicationBar.IsVisible = false;
            _overlayPopup.IsOpen = true;
        }

        void victoryOverlay_OnButtonRestart_Tap(object sender, EventArgs e)
        {
            Sapper.Restart();
            HideVictoryOverlay();
        }

        void victoryOverlay_OnButtonMainMenu_Tap(object sender, EventArgs e)
        {
            HideVictoryOverlay();
            NavigationService.GoBack();
        }

        private void HideVictoryOverlay()
        {
            if (_overlayPopup.Child is VictoryOverlay == false) throw new Exception("wrong overlay");

            this.LayoutRoot.Opacity = 1.0;
            this.ApplicationBar.IsVisible = true;
            _overlayPopup.IsOpen = false;
        }

        #endregion

        #region AppBar

        private void ShowSapperAppBar()
        {
            if (ApplicationBar == null) ApplicationBar = new ApplicationBar();
            else if (ApplicationBar.Buttons.Count > 0)
            {
                ApplicationBar.Buttons.Clear();
            }

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/AppBar/AppBarPause.png", UriKind.Relative);
            button1.Text = AppResources.AppBar_Button_pause;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(AppBarButton_Pause_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/AppBar/AppBarFlag.png", UriKind.Relative);
            button2.Text = AppResources.AppBar_Button_flag;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(AppBarButton_Flag_Click);
        }

        private void ShowGameOverAppBar()
        {
            if (ApplicationBar == null) ApplicationBar = new ApplicationBar();
            else if (ApplicationBar.Buttons.Count > 0)
            {
                ApplicationBar.Buttons.Clear();
            }

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/AppBar/AppBarRestart.png", UriKind.Relative);
            button1.Text = AppResources.AppBar_Button_restart;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += AppBarButton_Restart_Click;
            ApplicationBar.IsVisible = true;
        }

        void AppBarButton_Restart_Click(object sender, EventArgs e)
        {
            Sapper.Restart();
            HideGameOverOverlay();
        }

        private void AppBarButton_Flag_Click(object sender, EventArgs e)
        {
            if (Sapper.FlagMode == true)
            {
                Sapper.FlagMode = false;
                (sender as ApplicationBarIconButton).IconUri = new Uri("/Assets/AppBar/AppBarFlag.png", UriKind.Relative);
            }
            else
            {
                Sapper.FlagMode = true;
                (sender as ApplicationBarIconButton).IconUri = new Uri("/Assets/AppBar/AppBarReversedFlag.png", UriKind.Relative);
            }
        }

        private void AppBarButton_Pause_Click(object sender, EventArgs e)
        {
            Sapper.Pause();

            ShowPauseMenuOverlay();
        }

        #endregion
    }
}