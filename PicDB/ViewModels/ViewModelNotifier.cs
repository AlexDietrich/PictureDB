using System.ComponentModel;

namespace PicDB.ViewModels
{
    public class ViewModelNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies if a property is changed
        /// </summary>
        /// <param name="propName"></param>
        protected void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
