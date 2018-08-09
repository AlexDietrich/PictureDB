using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB;
using PicDB.ViewModels;

namespace Uebungen
{
    public class UEB3 : IUEB3
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

        public IDataAccessLayer GetDataAccessLayer()
        {
            return dataAccesLayer;
        }

        public ISearchViewModel GetSearchViewModel()
        {
            return new SearchViewModel();
        }
    }
}
