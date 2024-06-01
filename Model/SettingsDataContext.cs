using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;

namespace Sapper2.Model
{

    public class SettingsDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public SettingsDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the to-do items.
        public Table<LevelItem> Items;

    }

    [Table]
    public class SettingsItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public SettingsItem()
        {
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
        private bool _vibrationItem;

        [Column]
        public bool VibrationItem
        {
            get { return _vibrationItem; }
            set
            {
                if (_vibrationItem != value)
                {
                    NotifyPropertyChanging("VibrationItem");
                    _vibrationItem = value;
                    NotifyPropertyChanged("VibrationItem");
                }
            }
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
