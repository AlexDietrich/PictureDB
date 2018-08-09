using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces;

namespace PicDB
{
    class DataAccessLayerFactory
    {
        public string PicturePath = String.Empty;
        private static readonly DataAccessLayerFactory instance = new DataAccessLayerFactory();

        private DataAccessLayerFactory() { }

        public static DataAccessLayerFactory Instance => instance;

        public IDataAccessLayer getDataAccessLayer(bool Test)
        {
            if (Test)
            {
                return new MockDataAccessLayer();
            }
            return new DataAccessLayer();
        }
    }
}
