using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.ViewModels
{
    class PhotographerListViewModel : ViewModelNotifier, IPhotographerListViewModel
    {
        private IEnumerable<IPhotographerViewModel> _list;
        public IEnumerable<IPhotographerViewModel> List
        {
            get => _list;
            private set
            {
                _list = value;
                NotifyPropertyChanged("List");
            }
        }

        public IPhotographerViewModel CurrentPhotographer
        {
            get => CurrentPhotographer;
            set
            {
                CurrentPhotographer = value;
                NotifyPropertyChanged("CurrentPhotographer");
            }
        }

        public PhotographerListViewModel()
        {
            SynchronizePhotographers();
        }

        public void SynchronizePhotographers()
        {
            var bl = new BusinessLayer();
            var photographerModels = bl.GetPhotographers();
            var photogrViewModels = new ObservableCollection<IPhotographerViewModel>();
            foreach (var photographerModel in photographerModels)
            {
                photogrViewModels.Add(new PhotographerViewModel(photographerModel));
            }

            List = photogrViewModels;
        }
    }
}
