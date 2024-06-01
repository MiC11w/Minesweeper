using Sapper2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Sapper2
{
    public abstract class DataTemplateSelector : ContentControl
    {
        public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }

    public class SlotTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmptySlotTemplate
        {
            get;
            set;
        }

        public DataTemplate UsedSlotTemplate
        {
            get;
            set;
        }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            LevelItem levelItem = item as LevelItem; 

            levelItem.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(itemAux_PropertyChanged);

            return GetTemplate(levelItem, container);
        }

        private DataTemplate GetTemplate(LevelItem levelItem, DependencyObject container)
        {
            if (levelItem != null)
            {
                if (levelItem.UntouchedItem == true)
                {
                    return EmptySlotTemplate;
                }
                else
                {
                    return UsedSlotTemplate;
                }
            }

            return base.SelectTemplate(levelItem, container);
        }

        void itemAux_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // A property has changed, we need to reevaluate the template
            this.ContentTemplate = GetTemplate(sender as LevelItem, this);
        }
    }
}
