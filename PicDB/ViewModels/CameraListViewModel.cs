using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.ViewModels
{
    class CameraListViewModel : ViewModelNotifier, ICameraListViewModel
    {

        private IEnumerable<ICameraViewModel> _list;
        public IEnumerable<ICameraViewModel> List
        {
            get => _list;
            private set
            {
                _list = value;
                NotifyPropertyChanged("List");
            }
        }

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
