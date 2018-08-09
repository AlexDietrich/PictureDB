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

        public IEnumerable<IPictureModel> GetPictures()
        {
            return _dataAccessLayer.GetPictures(null, null, null, null);
        }

        public IEnumerable<IPictureModel> GetPictures(string namePart, IPhotographerModel photographerParts, IIPTCModel iptcParts,
            IEXIFModel exifParts)
        {
            return _dataAccessLayer.GetPictures(namePart, photographerParts, iptcParts, exifParts);
        }

        public IPictureModel GetPicture(int ID)
        {
            return _dataAccessLayer.GetPicture(ID);
        }

        public void Save(IPictureModel picture)
        {
            _dataAccessLayer.Save(picture);
        }

        public void DeletePicture(int ID)
        {
            _dataAccessLayer.DeletePicture(ID);
        }

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

        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            return _dataAccessLayer.GetPhotographers();
        }

        public IPhotographerModel GetPhotographer(int ID)
        {
            return _dataAccessLayer.GetPhotographer(ID);
        }

        public void Save(IPhotographerModel photographer)
        {
            _dataAccessLayer.Save(photographer);
        }

        public void DeletePhotographer(int ID)
        {
            _dataAccessLayer.DeletePhotographer(ID);
        }

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

        public void WriteIPTC(string filename, IIPTCModel iptc)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICameraModel> GetCameras()
        {
            return _dataAccessLayer.GetCameras();
        }

        public ICameraModel GetCamera(int ID)
        {
            return _dataAccessLayer.GetCamera(ID);
        }

        public void CurrentPictureChanged(IPictureViewModel currentPicture)
        {
            
        }
    }
}
