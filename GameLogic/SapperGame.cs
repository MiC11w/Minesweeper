using Sapper2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sapper2.GameLogic
{
    public class SapperGame
    {
        public event EventHandler FlagNumberChanged;
        public event EventHandler RevealedSquaresNumberChanged;
        public event EventHandler GameIsOver;
        public event EventHandler GameWin;
        public event EventHandler Tick;
        public enum SapperState { PreGame, GameOn, Paused, EndGame };

        public SapperState State { get; private set; }
        private Timer _timer;
        private GamePage parentPage;
        public int GameTime { get; private set; }
        public bool FlagMode { get; set; }
        private SapperBoard _sapperBoard;

        private int _flagNumber = 0;
        public int FlagNumber
        {
            get
            {
                return _flagNumber;
            }
            internal set
            {
                if (_flagNumber != value)
                {
                    _flagNumber = value;
                    FlagNumberChanged(this, new EventArgs());
                }
            }
        }

        private int _revealedSquaresNumber = 0;
        public int RevealedSquaresNumber
        {
            get
            {
                return _revealedSquaresNumber;
            }
            internal set
            {
                if (_revealedSquaresNumber != value)
                {
                    _revealedSquaresNumber = value;
                    RevealedSquaresNumberChanged(this, new EventArgs());
                }
            }
        }

        private SolidColorBrush _buttonColor;
        public SolidColorBrush ButtonColor
        {
            get
            {
                if( _buttonColor != null)
                    return _buttonColor;
                else
                    return App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
            }
            set
            {
                if (_buttonColor != value)
                {
                    _buttonColor = value;
                }
            }
        }

        public SapperGame(GamePage page, Grid gameGrid, int x, int y, int mines, SolidColorBrush color)
        {
            parentPage = page;
            State = SapperState.PreGame;
            ButtonColor = color;
            _sapperBoard = new SapperBoard(this, gameGrid, x, y, mines);

            FlagMode = false;
            GameTime = 0;
            _timer = new Timer(new TimerCallback(TimerTick));
        }

        //public void Scale(double newScale)
        //{
        //    _sapperBoard.Scale(newScale);
        //}

        public void Restart()
        {
            StopTimer();
            GameTime = 0;
            State = SapperState.PreGame;
            FlagMode = false;

            Tick(this, new EventArgs());
            FlagNumber = 0;
            RevealedSquaresNumber = 0;

            _sapperBoard.Restart();
        }

        public void Pause()
        {
            if (State == SapperState.GameOn)
            {
                StopTimer();
                State = SapperState.Paused;
            }
        }

        public void Resume()
        {
            if (State == SapperState.Paused)
            {
                StartTimer();
                State = SapperState.GameOn;
            }
        }

        internal void GameOver()
        {
            if (State == SapperState.GameOn)
            {
                StopTimer();
                GameIsOver(this, new EventArgs());
                State = SapperState.EndGame;
                _sapperBoard.Lock();
            }
        }

        internal void Start()
        {
            if (State == SapperState.PreGame)
            {
                StartTimer();
                State = SapperState.GameOn;
            }
        }

        internal void Win()
        {
            if (State == SapperState.GameOn)
            {
                StopTimer();
                GameWin(this, new EventArgs());
                State = SapperState.EndGame;
            }
        }

        private void StopTimer()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void StartTimer()
        {
            _timer.Change(1000, 1000);
        }

        private void TimerTick(object obj)
        {
            parentPage.Dispatcher.BeginInvoke(() => {
                GameTime++;
                Tick(this, new EventArgs());
            });
        }
    }
}
