using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Sapper2.Model;

namespace Sapper2.ViewModels
{
    public class LevelsViewModel : INotifyPropertyChanged
    {
        private LevelsDataContext _levelsDB;
        /// <summary>
        /// Constructor
        /// </summary>
        public LevelsViewModel(string ConnectionString)
        {
            _levelsDB = new LevelsDataContext(ConnectionString);
            IsDataLoaded = false;
        }

        public ObservableCollection<LevelItem> LevelItems { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            var DefaultLevelItemsInDB = from LevelItem lvl in _levelsDB.Items select lvl;

            // Query the database and load all to-do items.
            LevelItems = new ObservableCollection<LevelItem>(DefaultLevelItemsInDB);
            this.IsDataLoaded = true;
        }


        public void SubmitChanges()
        {
            _levelsDB.SubmitChanges();
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
    }
}
