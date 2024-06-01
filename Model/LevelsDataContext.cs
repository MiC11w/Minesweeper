using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace Sapper2.Model
{

    public class LevelsDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public LevelsDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the to-do items.
        public Table<LevelItem> Items;

    }

    [Table]
    public class LevelItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string _levelNameItem = "";

        public LevelItem()
        {
            Clear();
        }

        [Column(CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public string LevelNameItem
        {
            get { return _levelNameItem; }
            set
            {
                if (_levelNameItem != value)
                {
                    NotifyPropertyChanging("LevelNameItem");
                    _levelNameItem = value;
                    UntouchedItem = false;
                    NotifyPropertyChanged("LevelNameItem");
                    NotifyPropertyChanged("LevelName");
                }
            }
        }

        // Define ID: private field, public property, and database column.
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                }
            }

        }

        // Define item name: private field, public property, and database column.
        private int _minesNumberItem;

        [Column]
        public int MinesNumberItem
        {
            get { return _minesNumberItem; }
            set
            {
                if (_minesNumberItem != value)
                {
                    NotifyPropertyChanging("MinesNumberItem");
                    _minesNumberItem = value;
                    UntouchedItem = false;
                    NotifyPropertyChanged("MinesNumberItem");
                }
            }
        }

        // Define completion value: private field, public property, and database column.
        private int _boardXItem;

        [Column]
        public int BoardXItem
        {
            get { return _boardXItem; }
            set
            {
                if (_boardXItem != value)
                {
                    NotifyPropertyChanging("BoardXItem");
                    _boardXItem = value;
                    UntouchedItem = false;
                    NotifyPropertyChanged("BoardXItem");
                    NotifyPropertyChanged("BoardXY");
                }
            }
        }

        // Define completion value: private field, public property, and database column.
        private int _boardYItem;

        [Column]
        public int BoardYItem
        {
            get { return _boardYItem; }
            set
            {
                if (_boardYItem != value)
                {
                    NotifyPropertyChanging("BoardYItem");
                    _boardYItem = value;
                    UntouchedItem = false;
                    NotifyPropertyChanged("BoardYItem");
                    NotifyPropertyChanged("BoardXY");
                }
            }
        }

        // Highscore exist
        private bool _bestTimeExistsItem = false;

        [Column]
        public bool BestTimeExistsItem
        {
            get { return _bestTimeExistsItem; }
            private set
            {
                if (_bestTimeExistsItem != value)
                {
                    NotifyPropertyChanging("BestTimeExistsItem");
                    _bestTimeExistsItem = value;
                    NotifyPropertyChanged("BestTimeExistsItem");
                }
            }
        }

        // Highscore
        private int _bestTimeItem;

        [Column]
        public int BestTimeItem
        {
            get { return _bestTimeItem; }
            set
            {
                if (_bestTimeItem != value)
                {
                    NotifyPropertyChanging("BestTimeItem");
                    _bestTimeItem = value;
                    BestTimeExistsItem = true;
                    UntouchedItem = false;
                    NotifyPropertyChanged("BestTimeItem");
                    NotifyPropertyChanged("BestTime");
                }
            }
        }

        // Highscore
        private bool _untouchedItem;

        [Column]
        public bool UntouchedItem
        {
            get { return _untouchedItem; }
            private set
            {
                if (_untouchedItem != value)
                {
                    NotifyPropertyChanging("UntouchedItem");
                    _untouchedItem = value;
                    NotifyPropertyChanged("UntouchedItem");
                }
            }
        }

        //Color
        private string _color;

        [Column]
        public string Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    NotifyPropertyChanging("Color");
                    _color = value;
                    NotifyPropertyChanged("Color");
                }
            }
        }

        public SolidColorBrush ColorBrush 
        {
            get 
            {
                if (Color == null) return null;

                Color col = new Color();
                col.A = byte.Parse(Color.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                col.R = byte.Parse(Color.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                col.G = byte.Parse(Color.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                col.B = byte.Parse(Color.Substring(6, 2), NumberStyles.AllowHexSpecifier);

                return new SolidColorBrush(col);
            }
            set
            {
                SolidColorBrush brush = value;
                Color col = brush.Color;
                StringBuilder strb = new StringBuilder();
                strb.Append(col.A.ToString("X2"));
                strb.Append(col.R.ToString("X2"));
                strb.Append(col.G.ToString("X2"));
                strb.Append(col.B.ToString("X2"));
                this.Color = strb.ToString();
            }
        }

        public string LevelName
        {
            get
            {
                if (UntouchedItem == true) 
                    return " ";
                else if (LevelNameItem.Length == 0) 
                    return " ";
                else 
                    return LevelNameItem;
            }
        }

        public string BestTime 
        {
            get
            {
                if (BestTimeExistsItem == true)
                {
                    return TimeSpan.FromSeconds(BestTimeItem).ToString(@"mm\:ss");
                }
                else
                {
                    return "--:--";
                }
            }
        }

        public string BoardXY
        {
            get
            {
                StringBuilder strb = new StringBuilder();
                strb.Append(BoardXItem);
                strb.Append(" x ");
                strb.Append(BoardYItem);
                return strb.ToString();
            }
        }

        public void ClearTime()
        {
            BestTimeItem = 0;
            BestTimeExistsItem = false;
        }

        public void Clear()
        {
            LevelNameItem = "";
            MinesNumberItem = 0;
            BoardXItem = 10;
            BoardYItem = 10;
            BestTimeItem = 0;
            BestTimeExistsItem = false;
            UntouchedItem = true;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

}
