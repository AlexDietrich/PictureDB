using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB;
using PicDB.Models;
using PicDB.ViewModels;

namespace Uebungen
{
    public class UEB4 : IUEB4
    {
        private static readonly DataAccessLayerFactory Factory = DataAccessLayerFactory.Instance;
        private static IDataAccessLayer dataAccesLayer = Factory.getDataAccessLayer(true);
        public void HelloWorld()
        {
        }

        public IBusinessLayer GetBusinessLayer()
        {
            return new BusinessLayer(dataAccesLayer, Factory.PicturePath);
        }

        public void TestSetup(string picturePath)
        {
            Factory.PicturePath = picturePath;
        }

        public IEXIFModel GetEmptyEXIFModel()
        {
            return new EXIFModel();
        }

        public IEXIFViewModel GetEXIFViewModel(IEXIFModel mdl)
        {
            return new EXIFViewModel(mdl);
        }

        public IIPTCModel GetEmptyIPTCModel()
        {
            return new IPTCModel();
        }

        public IIPTCViewModel GetIPTCViewModel(IIPTCModel mdl)
        {
            return new IPTCViewModel(mdl);
        }

        public ICameraModel GetCameraModel(string producer, string make)
        {
            return new CameraModel(producer, make);
        }

        public ICameraViewModel GetCameraViewModel(ICameraModel mdl)
        {
            return new CameraViewModel(mdl);
        }
    }
}
