using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;
using PicDB;
using PicDB.Models;

namespace Uebungen
{
    public class UEB6 : IUEB6
    {
        private static readonly DataAccessLayerFactory Factory = DataAccessLayerFactory.Instance;
        public void HelloWorld()
        {
        }

        public IBusinessLayer GetBusinessLayer()
        {
            IDataAccessLayer dataAccesLayer = Factory.getDataAccessLayer(true);
            return new BusinessLayer(dataAccesLayer, Factory.PicturePath);
        }

        public void TestSetup(string picturePath)
        {
            Factory.PicturePath = picturePath;
        }

        public IPictureModel GetEmptyPictureModel()
        {
            return new PictureModel();
        }

        public IPhotographerModel GetEmptyPhotographerModel()
        {
            return new PhotographerModel();
        }
    }
}
