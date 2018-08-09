using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB.Models;

namespace PicDB.ViewModels
{
    class PictureListViewModel : ViewModelNotifier, IPictureListViewModel
    {
        public PictureListViewModel()
        {
            var bl = new BusinessLayer();
            bl.Sync(); //First Synch !!
            var pictures = bl.GetPictures();

            foreach (IPictureModel model in pictures)
            {
                _list.Add(new PictureViewModel((PictureModel)model));
            }
            int firstModelID = _list.First().ID;
            CurrentPicture = new PictureViewModel(bl.GetPicture(firstModelID));
        }

        public PictureListViewModel(IEnumerable<IPictureViewModel> p)
        {
            _list = new ObservableCollection<IPictureViewModel>(p);
        }

        public PictureListViewModel(IEnumerable<IPictureModel> pictures)
        {
            foreach (var pic in pictures)
            {
                var v = new PictureViewModel(pic);
                _list.Add(v);
            }
        }

        public IPictureViewModel CurrentPicture { get; set; }


        public IEnumerable<IPictureViewModel> List
        {
            get => _list;
            set
            {
                _list = (ObservableCollection<IPictureViewModel>)value;
                NotifyPropertyChanged("List");
            }
        }
        public IEnumerable<IPictureViewModel> PrevPictures { get; }

        public IEnumerable<IPictureViewModel> NextPictures { get; }

        public int Count { get; }

        public int CurrentIndex { get; set; }

        public string CurrentPictureAsString { get; }

        private ObservableCollection<IPictureViewModel> _list = new ObservableCollection<IPictureViewModel>();
    }
}