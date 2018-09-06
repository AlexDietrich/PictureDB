using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB.ViewModels;

namespace PicDB.Models
{
    public class MainWindowViewModel : ViewModelNotifier, IMainWindowViewModel
    {
        private readonly BusinessLayer _businessLayer = new BusinessLayer();

        private IPictureViewModel _currentPicture;
        public IPictureViewModel CurrentPicture
        {
            get => _currentPicture;
            set
            {
                if (_currentPicture != value && value != null)
                {
                    _currentPicture = new PictureViewModel(_businessLayer.GetPicture(value.ID));
                    ((PictureListViewModel)List).CurrentPicture = _currentPicture;
                    Title = "PicDB - " + _currentPicture.DisplayName;
                    NotifyPropertyChanged(nameof(CurrentPicture));
                }
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    return "PicDB";
                }
                return _title;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _title = value;
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }

        public IPictureListViewModel List { get; set; } = new PictureListViewModel();

        public ISearchViewModel Search { get; set; } = new SearchViewModel();

        public ICameraListViewModel CameraList { get; set; } = new CameraListViewModel();

        public IPhotographerListViewModel PhotographerList { get; set; } = new PhotographerListViewModel();

        public MainWindowViewModel()
        {
            CurrentPicture = List.CurrentPicture;
            Title = "PicDB - " + CurrentPicture.DisplayName;
        }

        public void SaveCurrentPicture()
        {
            _businessLayer.Save(new PictureModel(CurrentPicture));
        }

        internal void SaveGeneralInformation(CameraViewModel cameraViewmodel, PhotographerViewModel photographerViewModel)
        {
            ((PictureViewModel)CurrentPicture).Camera = cameraViewmodel ?? null;

            ((PictureViewModel)CurrentPicture).Photographer = photographerViewModel ?? null;
            SaveCurrentPicture();
        }

        //public ObservableCollection<> CreatePictureViewModelCollection()
        internal void SaveCamera(CameraViewModel camera)
        {
            var camModel = new CameraModel(camera);
            _businessLayer.SaveCamera(camModel);
            var cameraList = (CameraListViewModel) CameraList;
            cameraList.SynchronizeCameras();
        }

        internal void UpdateCamera(ICameraViewModel cameraViewModel)
        {
            _businessLayer.UpdateCamera(new CameraModel(cameraViewModel));
            ((CameraListViewModel)CameraList).SynchronizeCameras();
        }

        internal void DeleteCamera(int ID)
        {
            _businessLayer.DeleteCamera(ID);
            ((CameraListViewModel)CameraList).SynchronizeCameras();
        }

        public void UpdatePhotographer(PhotographerViewModel photographerViewModel)
        {
            _businessLayer.Save(new PhotographerModel(photographerViewModel));
            ((PhotographerListViewModel)PhotographerList).SynchronizePhotographers();
        }

        public void DeletePhotographer(int id)
        {
            _businessLayer.DeletePhotographer(id);
            ((PhotographerListViewModel)PhotographerList).SynchronizePhotographers();
        }

        public void SavePhotographer(PhotographerViewModel photographer)
        {
            _businessLayer.Save(new PhotographerModel(photographer));
            var photographerlist = (PhotographerListViewModel)PhotographerList;
            photographerlist.SynchronizePhotographers();
        }
    }
}
