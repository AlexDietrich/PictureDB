using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB.Models;

namespace PicDB
{
    class BusinessLayer : IBusinessLayer
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        private readonly string _pathFolder;
        public BusinessLayer()
        {
            _dataAccessLayer = new DataAccessLayer();
            GlobalInformation.ReadConfigFile();
            _pathFolder = GlobalInformation.Path;
        }

        public BusinessLayer(IDataAccessLayer al, string picturesPathFolderPath)
        {
            this._dataAccessLayer = al;
            this._pathFolder = picturesPathFolderPath;
        }

        /// <summary>
        /// Get all Pictures from the Picture Database
        /// </summary>
        /// <returns>Dictionary with Pictures</returns>
        public IEnumerable<IPictureModel> GetPictures()
        {
            return _dataAccessLayer.GetPictures(null, null, null, null);
        }

        /// <summary>
        /// Get a list of specific pictures from the Database
        /// </summary>
        /// <param name="namePart">Part of the name to search for the picture</param>
        /// <param name="photographerParts">part of the name from the photographer of the picture</param>
        /// <param name="iptcParts">parts of the iptc - information saved on the picture</param>
        /// <param name="exifParts">parts of the exif - information saved on the picture</param>
        /// <returns>dictionary with pictures</returns>
        public IEnumerable<IPictureModel> GetPictures(string namePart, IPhotographerModel photographerParts, IIPTCModel iptcParts,
            IEXIFModel exifParts)
        {
            return _dataAccessLayer.GetPictures(namePart, photographerParts, iptcParts, exifParts);
        }

        /// <summary>
        /// Get a specific picture from the Database
        /// </summary>
        /// <param name="ID">ID of the Picture in the database</param>
        /// <returns>picture</returns>
        public IPictureModel GetPicture(int ID)
        {
            return _dataAccessLayer.GetPicture(ID);
        }

        /// <summary>
        /// Save a picture in the database
        /// </summary>
        /// <param name="picture">picture which should be saved in the database</param>
        public void Save(IPictureModel picture)
        {
            _dataAccessLayer.Save(picture);
        }

        /// <summary>
        /// Delete a picture of the database
        /// </summary>
        /// <param name="ID">Database id of the picture</param>
        public void DeletePicture(int ID)
        {
            _dataAccessLayer.DeletePicture(ID);
        }

        /// <summary>
        /// Sync the pictures from the folder with the pictures from the database
        /// </summary>
        public void Sync()
        {
            //Alle Filenamen holen die sich im angegebenen Verzeichnis finden
            IEnumerable<string> pathFiles = Directory.EnumerateFiles(GlobalInformation.Path);
            //Erstelle eine Liste und füge mit einer foreach Schleife die gefunden Files von pathFiles und füge die einzelnen Elemente der Liste hinzu
            var files = new HashSet<string>(pathFiles.Select(Path.GetFileName)); 
            
            //Erstelle eine Liste von allen Bilder, welche sich in der Datenbank befinden
            List<IPictureModel> pictures = _dataAccessLayer.GetPictures(null, null, null, null).ToList();
 
            foreach (var pictureModel in pictures)
            {
                //Falls das Bild aus der Datenbank nicht im Ordner ist, Lösche es aus der Datenbank
                if (!files.Contains(pictureModel.FileName))
                {
                    DeletePicture(pictureModel.ID);
                }
                //falls es bereits synchronisiert ist dann lösche es aus den zu synchronisierenden Files raus
                else
                {
                    files.Remove(pictureModel.FileName);
                }
            }

            //Alle Files die noch nicht mit dem Ordner synchronisiert sind, zur Datenbank hinzufügen
            foreach (var filename in files)
            {
                IPictureModel pictureModel = new PictureModel(filename);
                pictureModel.EXIF = ExtractEXIF(filename);
                pictureModel.IPTC = ExtractIPTC(filename);
                Save(pictureModel);
            }
        }

        /// <summary>
        /// Returns a list of ALL Photographers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            return _dataAccessLayer.GetPhotographers();
        }

        /// <summary>
        /// Returns ONE Photographer
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IPhotographerModel GetPhotographer(int ID)
        {
            return _dataAccessLayer.GetPhotographer(ID);
        }

        /// <summary>
        /// Saves all changes.
        /// </summary>
        /// <param name="photographer"></param>
        public void Save(IPhotographerModel photographer)
        {
            _dataAccessLayer.Save(photographer);
        }

        /// <summary>
        /// Deletes a Photographer. A Exception is thrown if a Photographer is still linked to a picture.
        /// </summary>
        /// <param name="ID"></param>
        public void DeletePhotographer(int ID)
        {
            _dataAccessLayer.DeletePhotographer(ID);
        }

        /// <summary>
        /// Extracts IPTC information from a picture. NOTE: You may simulate the action.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IIPTCModel ExtractIPTC(string filename)
        {
            var iptcData = new IPTCModel();
            IEnumerable<string> pathFiles = Directory.EnumerateFiles(GlobalInformation.Path);
            if (!pathFiles.Contains(Path.Combine(GlobalInformation.Path, filename))) throw new FileNotFoundException();
            iptcData.ByLine = "ByLine";
            iptcData.Caption = "caption";
            iptcData.CopyrightNotice = "this is my shit - bro!";
            iptcData.Headline = "I'm the Head";
            iptcData.Keywords = "Blame on me";
            return iptcData;
        }

        /// <summary>
        /// Extracts EXIF information from a picture. NOTE: You may simulate the action.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IEXIFModel ExtractEXIF(string filename)
        {
            var exifData = new EXIFModel();
            IEnumerable<string> pathFiles = Directory.EnumerateFiles(GlobalInformation.Path);
            if (!pathFiles.Contains(Path.Combine(GlobalInformation.Path, filename))) throw new FileNotFoundException();
            exifData.ExposureProgram = ExposurePrograms.CreativeProgram;
            exifData.ExposureTime = 10;
            exifData.FNumber = 2;
            exifData.Flash = true;
            exifData.ISOValue = 8008;
            exifData.Make = "Make";
            return exifData;
        }

        /// <summary>
        /// Writes IPTC information back to a picture. NOTE: You may simulate the action.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="iptc"></param>
        public void WriteIPTC(string filename, IIPTCModel iptc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of ALL Cameras.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ICameraModel> GetCameras()
        {
            return _dataAccessLayer.GetCameras();
        }

        /// <summary>
        /// Returns ONE Camera
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ICameraModel GetCamera(int ID)
        {
            return _dataAccessLayer.GetCamera(ID);
        }

        /// <summary>
        /// Useless function
        /// </summary>
        /// <param name="currentPicture"></param>
        public void CurrentPictureChanged(IPictureViewModel currentPicture)
        {
            
        }

        /// <summary>
        /// Delete a specific camera of the database
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteCamera(int ID)
        {
            ((DataAccessLayer)_dataAccessLayer).DeleteCamera(ID);
        }

        /// <summary>
        /// Update an existing camera in the database
        /// </summary>
        /// <param name="cameraModel"></param>
        public void UpdateCamera(ICameraModel cameraModel)
        {
            ((DataAccessLayer)_dataAccessLayer).UpdateCamera(cameraModel);
        }

        /// <summary>
        /// Save a camera in the database
        /// </summary>
        /// <param name="camera"></param>
        public void SaveCamera(CameraModel camera)
        {
            var dal = (DataAccessLayer) _dataAccessLayer;
            dal.SaveCamera(camera);
        }

        public Dictionary<string, int> GetTagCount()
        {
            return ((DataAccessLayer) _dataAccessLayer).GetTagCount();
        }
    }
}
