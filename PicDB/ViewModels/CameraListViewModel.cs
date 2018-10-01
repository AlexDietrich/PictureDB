using System.Collections.Generic;
using System.Collections.ObjectModel;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.ViewModels
{
    class CameraListViewModel : ViewModelNotifier, ICameraListViewModel
    {

        private IEnumerable<ICameraViewModel> _list;

        /// <summary>
        /// List of all CameraListViewModel
        /// </summary>
        public IEnumerable<ICameraViewModel> List
        {
            get => _list;
            private set
            {
                _list = value;
                NotifyPropertyChanged("List");
            }
        }

        /// <summary>
        /// The currently selected CameraListViewModel
        /// </summary>
        public ICameraViewModel CurrentCamera
        {
            get => CurrentCamera;
            set
            {
                CurrentCamera = value;
                NotifyPropertyChanged("CurrentCamera");
            }
        }

        public CameraListViewModel()
        {
            SynchronizeCameras();
        }

        /// <summary>
        /// Syncs the cameras from the db with the list 
        /// </summary>
        public void SynchronizeCameras()
        {
            var bl = new BusinessLayer();
            var cameraModels = bl.GetCameras();
            var cameraViewModels = new ObservableCollection<ICameraViewModel>();
            foreach (var cameraModel in cameraModels)
            {
                cameraViewModels.Add(new CameraViewModel(cameraModel));
            }

            List = cameraViewModels;
        }
    }
}
