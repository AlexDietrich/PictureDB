using BIF.SWE2.Interfaces.ViewModels;
using PicDB.ViewModels;

namespace PicDB.Models
{
    class MainWindowViewModel : ViewModelNotifier, IMainWindowViewModel
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
            ((PictureViewModel)CurrentPicture).Camera =  cameraViewmodel;
            ((PictureViewModel)CurrentPicture).Photographer = photographerViewModel;
            SaveCurrentPicture();
        }

        //public ObservableCollection<> CreatePictureViewModelCollection()
    }
}
