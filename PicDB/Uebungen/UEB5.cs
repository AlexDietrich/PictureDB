using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB;
using PicDB.Models;
using PicDB.ViewModels;

namespace Uebungen
{
    public class UEB5 : IUEB5
    {
        private static readonly DataAccessLayerFactory Factory = DataAccessLayerFactory.Instance;
        private static readonly IDataAccessLayer DataAccesLayer = Factory.getDataAccessLayer(true);
        public void HelloWorld()
        {
        }

        public IBusinessLayer GetBusinessLayer()
        {
            return new BusinessLayer(DataAccesLayer, Factory.PicturePath);
        }

        public void TestSetup(string picturePath)
        {
            Factory.PicturePath = picturePath;
        }

        public IPhotographerModel GetEmptyPhotographerModel()
        {
            return new PhotographerModel();
        }

        public IPhotographerViewModel GetPhotographerViewModel(IPhotographerModel mdl)
        {
            return new PhotographerViewModel(mdl);
        }

        public ICameraModel GetEmptyCameraModel()
        {
            return new CameraModel();
        }

        public ICameraViewModel GetCameraViewModel(ICameraModel mdl)
        {
            return new CameraViewModel(mdl);
        }
    }
}
