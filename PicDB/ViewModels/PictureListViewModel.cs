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
        private readonly BusinessLayer bl = new BusinessLayer();

        public PictureListViewModel()
        {
            bl.Sync(); //First Synch !!
            var pictures = bl.GetPictures();

            foreach (IPictureModel model in pictures)
            {
                _list.Add(new PictureViewModel((PictureModel)model));
            }
            _backupList = new ObservableCollection<IPictureViewModel>(_list);

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

        private ObservableCollection<IPictureViewModel> _backupList;

        private ObservableCollection<IPictureViewModel> _list = new ObservableCollection<IPictureViewModel>();

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

        public void ResetList()
        {
            _list = new ObservableCollection<IPictureViewModel>(_backupList);
        }

        public void SyncAndUpdatePictureList()
        {
            bl.Sync();
            var pictures = bl.GetPictures();
            CurrentPicture = null;
            _list.Clear();
            foreach (IPictureModel model in pictures)
            {
                _list.Add(new PictureViewModel((PictureModel)model));
            }
            _backupList = new ObservableCollection<IPictureViewModel>(_list);

            int firstModelID = _list.First().ID;
            CurrentPicture = new PictureViewModel(bl.GetPicture(firstModelID));
        }
    }
}