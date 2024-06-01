using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Sapper2.GameLogic
{
    class SapperBoard
    {
        private SapperSquare[,] _squares;
        private int _mines;
        private int _width;
        private int _height;
        private SapperGame _sapperGame;

        internal Grid GameGrid { get; set; }
        internal int FlagNumber 
        {
            get
            {
                return _sapperGame.FlagNumber;
            }
            set
            {
                _sapperGame.FlagNumber = value;
            }
        }

        internal SapperBoard(SapperGame sapperGame, Grid gameGrid, int width, int height, int mines)
        {
            _sapperGame = sapperGame;
            _width = width;
            _height = height;
            _mines = mines;
            //setting grid
            GameGrid = gameGrid;
            GameGrid.RowDefinitions.Clear();
            for (int i = 0; i < height; i++) GameGrid.RowDefinitions.Add(new RowDefinition());
            GameGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < width; i++) GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            GameGrid.Margin = new Thickness(30);

            //Creating squares
            _squares = new SapperSquare[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SapperSquare tmpSqr = new SapperSquare(this, x, y, _sapperGame.ButtonColor);

                    tmpSqr.TapEH += SapperSquare_OnTap;
                    _squares[x, y] = tmpSqr;
                    GameGrid.Children.Add(tmpSqr);
                }
            }

            PlaceMines();
            UnLock();
        }

        internal void SapperSquare_OnTap(object sender, EventArgs e)
        {
            SapperSquare clickedSquare = sender as SapperSquare;

            if (_sapperGame.State == SapperGame.SapperState.PreGame) _sapperGame.Start();

            if (_sapperGame.FlagMode == false)
            {
                if (clickedSquare.State == SapperSquare.SquareState.Flaged)
                {
                    return;
                }
                else
                {
                    if (clickedSquare.Mined)
                    {
                        clickedSquare.Explode();
                        GameOver();
                    }
                    else
                    {
                        Reveal(clickedSquare);
                        CountRevealedSquares();
                        CheckForVictory();
                    }
                }
            }
            else
            {
                if (clickedSquare.State == SapperSquare.SquareState.Flaged)
                {
                    clickedSquare.UnFlag();
                }
                else
                {
                    clickedSquare.Flag();
                }
            }
        }

        //internal void Scale(double newScale)
        //{
        //    double currentSize = _squares[0, 0].SqrButton.Width;
        //    double scaledSize = currentSize * newScale;
        //    foreach (SapperSquare sqr in _squares)
        //    {
        //        //sqr.SqrButton.RenderTransform = new ScaleTransform() { ScaleX = newScale, ScaleY = newScale };
        //        sqr.SqrButton.Width = scaledSize;
        //        sqr.SqrButton.Height = scaledSize;
        //    }
        //}

        internal void Restart()
        {
            foreach(SapperSquare sqr in _squares)
            {
                sqr.Restart();
            }
            PlaceMines();
            UnLock();
        }

        internal void Lock()
        {
            GameGrid.IsHitTestVisible = false;
        }

        internal void UnLock()
        {
            GameGrid.IsHitTestVisible = true;
        }

        internal void CountRevealedSquares()
        {
            int revealedNumber = 0;
            foreach (SapperSquare sqr in _squares)
            {
                if (sqr.State == SapperSquare.SquareState.Revealed) revealedNumber++;
            }
            _sapperGame.RevealedSquaresNumber = revealedNumber;
        }

        private SapperSquare[] FindNeighbourSquares(int x, int y)
        {
            List<SapperSquare> neighbours = new List<SapperSquare>();

            for (int tmpx = x - 1; tmpx < x + 2; tmpx++)
                for (int tmpy = y - 1; tmpy < y + 2; tmpy++)
                {
                    if (tmpx == x && tmpy == y) continue;
                    //checking if coordinates are on board
                    if (tmpx >= 0 && tmpx < _width &&
                        tmpy >= 0 && tmpy < _height)
                    {
                        neighbours.Add(_squares[tmpx, tmpy]);
                    }
                }

            return neighbours.ToArray();
        }

        private void Reveal(SapperSquare sqr)
        {
            sqr.Reveal();
            int neighbourMinesNumber = 0;
            SapperSquare[] neighbourSquares = FindNeighbourSquares(sqr.X, sqr.Y);

            foreach (SapperSquare nSqr in neighbourSquares)
            {
                if (nSqr.Mined == true) neighbourMinesNumber++;
            }

            if (neighbourMinesNumber != 0) 
            {
                sqr.AddNumber(neighbourMinesNumber);
                return;
            }

            foreach (SapperSquare nSqr in neighbourSquares)
            {
                if (nSqr.State == SapperSquare.SquareState.Normal) Reveal(nSqr);
            }
        }

        private void GameOver()
        {
            //show all mines
            foreach (SapperSquare sqr in _squares)
            {
                if (sqr.Mined == true)
                    sqr.ShowMine();
            }

            _sapperGame.GameOver();
        }

        private void CheckForVictory()
        {
            int MineFreeSquares = _width * _height - _mines;
            if (MineFreeSquares == _sapperGame.RevealedSquaresNumber) _sapperGame.Win();
        }
        
        /// <summary>
        /// random set squares as mined
        /// </summary>
        private void PlaceMines()
        {
            if (_width * _height < _mines) throw new Exception("too much mines!");

            Random r = new Random();
            PointEqualityComparer pointComparer = new PointEqualityComparer();
            List<Point> places = new List<Point>();

            for (int i = 0; i < _mines; i++)
            {
                Point tmp = new Point(r.Next(0, _width), r.Next(0, _height));
                if (places.Contains<Point>(tmp, pointComparer))
                {
                    i--;
                    continue;
                }

                places.Add(tmp);
            }

            foreach (Point p in places)
            {
                _squares[(int)p.X, (int)p.Y].Mined = true;
            }
        }
    }

    internal class PointEqualityComparer : IEqualityComparer<Point>
    {
        public bool Equals(Point p1, Point p2)
        {
            return (p1.X == p2.X & p1.Y == p2.Y);
        }

        public int GetHashCode(Point px)
        {
            return (1000 * px.X + px.Y).GetHashCode();
        }
    }
}
