using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileWatcher.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public virtual void Dispose()
        {
            
        }
    }
}
