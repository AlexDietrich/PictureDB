using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;

namespace PicDB
{
    class DataAccessLayer : IDataAccessLayer
    {
        private static string ConnectionString =
            "Data Source=DESKTOP-V42FU8U;Initial Catalog=PicDB;Integrated Security=True";
        private SqlConnection _connection;

        public DataAccessLayer()
        {
            try
            {
                this._connection = new SqlConnection(ConnectionString);
            }
            catch(Exception ex)
            {
                //Write exception details in log file 

            }
        }

        public IEnumerable<IPictureModel> GetPictures(string namePart, IPhotographerModel photographerParts, IIPTCModel iptcParts,
            IEXIFModel exifParts)
        {
            //select PictureModel.ID, FileName, EXIFModel.Make, FNumber, ExposureTime, ISOValue, Flash, ExposureProgram, Keywords, ByLine, CopyrightNotice, Headline, Caption, fk_Camera, fk_Photographer from PictureModel
            //    inner join EXIFModel on fk_EXIF = ExifModel.ID
            //    inner join IPTCModel on fk_IPTC = IPTCModel.ID
            //    inner join CameraModel on fk_Camera = CameraModel.ID
            //    inner join PhotographerModel on fk_Photographer = PhotographerModel.ID;


            throw new NotImplementedException();
        }

        public IPictureModel GetPicture(int ID)
        {
            //select PictureModel.ID, FileName, EXIFModel.Make, FNumber, ExposureTime, ISOValue, Flash, ExposureProgram, Keywords, ByLine, CopyrightNotice, Headline, Caption, fk_Camera, fk_Photographer from PictureModel
            //    inner join EXIFModel on fk_EXIF = ExifModel.ID
            //    inner join IPTCModel on fk_IPTC = IPTCModel.ID
            //    inner join CameraModel on fk_Camera = CameraModel.ID
            //    inner join PhotographerModel on fk_Photographer = PhotographerModel.ID;
            //    where PictureModel.ID = ID;
            throw new NotImplementedException();
        }

        public void Save(IPictureModel picture)
        {
            throw new NotImplementedException();
        }

        public void DeletePicture(int ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            throw new NotImplementedException();
        }

        public IPhotographerModel GetPhotographer(int ID)
        {
            throw new NotImplementedException();
        }

        public void Save(IPhotographerModel photographer)
        {
            throw new NotImplementedException();
        }

        public void DeletePhotographer(int ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICameraModel> GetCameras()
        {
            throw new NotImplementedException();
        }

        public ICameraModel GetCamera(int ID)
        {
            throw new NotImplementedException();
        }
    }
}
