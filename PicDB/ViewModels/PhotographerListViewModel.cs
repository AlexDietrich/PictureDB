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
        public IEnumerable<IPhotographerViewModel> List { get; }

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
