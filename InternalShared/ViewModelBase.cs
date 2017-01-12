using System.ComponentModel;

namespace InternalShared
{
    /// <summary>
    /// Bazowa klasa wszystkich klas typu ViewModel
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {

        }

        public virtual string DisplayName { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var args = new PropertyChangedEventArgs(propertyName);
                handler(this, args);
            }
        }
    }
}
