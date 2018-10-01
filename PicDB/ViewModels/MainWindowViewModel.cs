using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB.ViewModels;

namespace PicDB.Models
{
    public class MainWindowViewModel : ViewModelNotifier, IMainWindowViewModel
    {
        private readonly BusinessLayer _businessLayer = new BusinessLayer();

        private IPictureViewModel _currentPicture;
        
        /// <summary>
        /// The current picture ViewModel
        /// </summary>
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

        /// <summary>
        /// ViewModel with a list of all Pictures
        /// </summary>
        public IPictureListViewModel List { get; set; } = new PictureListViewModel();

        /// <summary>
        /// Search ViewModel
        /// </summary>
        public ISearchViewModel Search { get; set; } = new SearchViewModel();

        /// <summary>
        /// List with cameras
        /// </summary>
        public ICameraListViewModel CameraList { get; set; } = new CameraListViewModel();

        /// <summary>
        /// List with Photographers
        /// </summary>
        public IPhotographerListViewModel PhotographerList { get; set; } = new PhotographerListViewModel();

        public MainWindowViewModel()
        {
            CurrentPicture = List.CurrentPicture;
            Title = "PicDB - " + CurrentPicture.DisplayName;
            var testy = new PDFReport();
            testy.CreateReport(CurrentPicture);
            testy.CreateReport(List, "on");
        }

        /// <summary>
        /// Save the current picture
        /// </summary>
        public void SaveCurrentPicture()
        {
            _businessLayer.Save(new PictureModel(CurrentPicture));
        }

        /// <summary>
        /// Save general information about the picture
        /// </summary>
        /// <param name="cameraViewmodel"></param>
        /// <param name="photographerViewModel"></param>
        internal void SaveGeneralInformation(CameraViewModel cameraViewmodel, PhotographerViewModel photographerViewModel)
        {
            ((PictureViewModel)CurrentPicture).Camera = cameraViewmodel ?? null;

            ((PictureViewModel)CurrentPicture).Photographer = photographerViewModel ?? null;
            SaveCurrentPicture();
        }

        /// <summary>
        /// Save the camera 
        /// </summary>
        /// <param name="camera"></param>
        //public ObservableCollection<> CreatePictureViewModelCollection()
        internal void SaveCamera(CameraViewModel camera)
        {
            var camModel = new CameraModel(camera);
            _businessLayer.SaveCamera(camModel);
            var cameraList = (CameraListViewModel) CameraList;
            cameraList.SynchronizeCameras();
        }

        /// <summary>
        /// Update the camera
        /// </summary>
        /// <param name="cameraViewModel"></param>
        internal void UpdateCamera(ICameraViewModel cameraViewModel)
        {
            _businessLayer.UpdateCamera(new CameraModel(cameraViewModel));
            ((CameraListViewModel)CameraList).SynchronizeCameras();
        }

        /// <summary>
        /// Delete the camera
        /// </summary>
        /// <param name="ID"></param>
        internal void DeleteCamera(int ID)
        {
            _businessLayer.DeleteCamera(ID);
            ((CameraListViewModel)CameraList).SynchronizeCameras();
        }

        /// <summary>
        /// Update the photographer
        /// </summary>
        /// <param name="photographerViewModel"></param>
        public void UpdatePhotographer(PhotographerViewModel photographerViewModel)
        {
            _businessLayer.Save(new PhotographerModel(photographerViewModel));
            ((PhotographerListViewModel)PhotographerList).SynchronizePhotographers();
        }

        /// <summary>
        /// Delete the photographer
        /// </summary>
        /// <param name="id"></param>
        public void DeletePhotographer(int id)
        {
            _businessLayer.DeletePhotographer(id);
            ((PhotographerListViewModel)PhotographerList).SynchronizePhotographers();
        }

        /// <summary>
        /// Save the photographer
        /// </summary>
        /// <param name="photographer"></param>
        public void SavePhotographer(PhotographerViewModel photographer)
        {
            _businessLayer.Save(new PhotographerModel(photographer));
            var photographerlist = (PhotographerListViewModel)PhotographerList;
            photographerlist.SynchronizePhotographers();
        }
    }
}
