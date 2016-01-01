using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AmazingGeoRace.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(ref T value, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(value, newValue))
                return;
            value = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}