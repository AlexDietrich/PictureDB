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

        /// <summary>
        /// List of all PhotographerViewModel
        /// </summary>
        public IEnumerable<IPhotographerViewModel> List
        {
            get => _list;
            private set
            {
                _list = value;
                NotifyPropertyChanged("List");
            }
        }

        /// <summary>
        /// The currently selected PhotographerViewModel
        /// </summary>
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

        /// <summary>
        /// Syncs the Photographer from the db with the list
        /// </summary>
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
