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

        /// <summary>
        /// ViewModel of the current picture
        /// </summary>
        public IPictureViewModel CurrentPicture { get; set; }

        private ObservableCollection<IPictureViewModel> _backupList;

        private ObservableCollection<IPictureViewModel> _list = new ObservableCollection<IPictureViewModel>();

        /// <summary>
        /// List of all PictureViewModels
        /// </summary>
        public IEnumerable<IPictureViewModel> List
        {
            get => _list;
            set
            {
                _list = (ObservableCollection<IPictureViewModel>)value;
                NotifyPropertyChanged("List");
            }
        }

        /// <summary>
        /// All prev. pictures to the current selected picture.
        /// </summary>
        public IEnumerable<IPictureViewModel> PrevPictures { get; }

        /// <summary>
        /// All next pictures to the current selected picture.
        /// </summary>
        public IEnumerable<IPictureViewModel> NextPictures { get; }

        /// <summary>
        /// Number of all images
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// The current Index, 1 based
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// {CurrentIndex} of {Cout}
        /// </summary>
        public string CurrentPictureAsString { get; }

        public void ResetList()
        {
            _list = new ObservableCollection<IPictureViewModel>(_backupList);
        }

        /// <summary>
        /// Syncs and updates the picturelist
        /// </summary>
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