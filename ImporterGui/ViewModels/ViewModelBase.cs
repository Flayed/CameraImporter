using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImporterGui.ViewModels
{
    public class ViewModelBase :INotifyPropertyChanged
    {
        public void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (property != null && property.Equals(value))
                return;
            property = value;
            NotifyPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
