using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Phone.Controls;


namespace Sapper2.GameLogic
{
    class SapperSquare : Button
    {
        private const int buttonSize = 52;

        internal enum SquareState { Normal, Flaged, Revealed, Exploded };
        private SapperBoard _sapperBoard;
        private DispatcherTimer _holdTimer;
        private bool _hold;
        private bool _dragLeave;

        internal event EventHandler TapEH;

        internal SquareState State { get; private set; }
        internal int X { get; private set; }
        internal int Y { get; private set; }
        internal bool Mined { get; set; }


        internal SapperSquare(SapperBoard sapperBoard, int x, int y, SolidColorBrush color)
        {
            _sapperBoard = sapperBoard;
            _holdTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 222) }; //222ms
            _holdTimer.Tick += _holdTimer_Tick;
            _hold = false;
            X = x;
            Y = y;
            State = SquareState.Normal;

            // button
            
            this.Style = App.Current.Resources["SapperGameButtonStyle"] as Style;
            this.Width = buttonSize;
            this.Height = buttonSize;
            this.SetValue(Grid.RowProperty, y);
            this.SetValue(Grid.ColumnProperty, x);

        }

        void _holdTimer_Tick(object sender, EventArgs e)
        {
            _holdTimer.Stop();
            if (_hold == true)
            {
                if (State == SquareState.Normal) this.Flag();
                else this.UnFlag();
                _hold = false;
            }
            
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TapEH == null) return;
            if (_dragLeave == true || _hold == false) return;

            TapEH(this, new EventArgs());
            _hold = false;
            
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            ////hold
            _hold = true;
            _dragLeave = false;
            _holdTimer.Start();
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            _dragLeave = true;
            _hold = false;
            base.OnMouseLeave(e);
        }

        internal void Restart()
        {
            this.State = SquareState.Normal;
            Mined = false;
            this.Content = null;
            this.IsEnabled = true;
        }

        internal void Flag()
        {
            if (State != SquareState.Normal) return;
            State = SquareState.Flaged;

            Uri flagUri = new Uri("/Assets/Square/Flag.png", UriKind.Relative);
            this.Content = new Image() { Source = new BitmapImage(flagUri) };

            _sapperBoard.FlagNumber++;
            _sapperBoard.CountRevealedSquares();
        }

        internal void UnFlag()
        {
            if (State != SquareState.Flaged) return;
            State = SquareState.Normal;

            this.Content = null;

            _sapperBoard.FlagNumber--;
        }

        internal void Reveal()
        {
            if (State != SquareState.Normal) return;
            State = SquareState.Revealed;

            this.IsEnabled = false;
        }

        internal void AddNumber(int nr)
        {
            if (State != SquareState.Revealed) return;
            State = SquareState.Revealed;

            this.Foreground = NumberColor(nr);
            this.Content = nr.ToString();
        }

        internal void ShowMine()
        {
            if (State != SquareState.Normal && State != SquareState.Flaged) return;

            if (State == SquareState.Flaged)
                this.Content = new Image() { Source = new BitmapImage(new Uri("/Assets/Square/FlagedMine.png", UriKind.Relative)) };
            else
                this.Content = new Image() { Source = new BitmapImage(new Uri("/Assets/Square/Mine.png", UriKind.Relative)) };

            State = SquareState.Exploded;
        }

        internal void Explode()
        {
            if (State != SquareState.Normal && State != SquareState.Flaged) return;
            State = SquareState.Exploded;

            this.Content = new Image() { Source = new BitmapImage(new Uri("/Assets/Square/Explosion.png", UriKind.Relative)) };
        }

        private SolidColorBrush NumberColor(int nr)
        {
            switch (nr)
            {
                case 1: return new SolidColorBrush(Colors.Green);
                case 2: return new SolidColorBrush(new Color() { R = 0x64, G = 0x95, B = 0xED, A = 255 }); //cornflowerblue #6495ED
                case 3: return new SolidColorBrush(new Color() { R = 0xFF, G = 0xA5, B = 0x00, A = 255 }); //orange #FFA500
                case 4: return new SolidColorBrush(Colors.Brown);
                default: return new SolidColorBrush(Colors.Red);
            }
        }
    }
}
