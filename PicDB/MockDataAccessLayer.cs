using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;
using PicDB.Models;

namespace PicDB
{
    class MockDataAccessLayer : IDataAccessLayer
    {
        private readonly DataAccessLayerFactory _dataAccessLayerFactory = DataAccessLayerFactory.Instance;

        private static long _maxId = 1; 

        private readonly List<IPhotographerModel> _photographerModels = new List<IPhotographerModel>(collection: new[]
        {
            new PhotographerModel(),
            new PhotographerModel(),
            new PhotographerModel(),
        });
        private readonly List<ICameraModel> _cameraModels = new List<ICameraModel>(collection: new[]
        {
            new CameraModel(),
            new CameraModel(),
        });
        private readonly List<IPictureModel> _pictureModels = new List<IPictureModel>();

        public IEnumerable<IPictureModel> GetPictures(string namePart, IPhotographerModel photographerParts, IIPTCModel iptcParts,
            IEXIFModel exifParts)
        {
            List<IPictureModel> pictureModels = new List<IPictureModel>();
            //Alle Filenamen holen die sich im angegebenen Verzeichnis finden
            IEnumerable<string> pathFiles = Directory.EnumerateFiles(_dataAccessLayerFactory.PicturePath);
            //Erstelle eine Liste und füge mit einer foreach Schleife die gefunden Files von pathFiles und füge die einzelnen Elemente der Liste hinzu
            List<string> files = pathFiles.Select(Path.GetFileName).ToList();
            if (string.IsNullOrEmpty(namePart))
            {
                //Alle Pictures ausgeben
                foreach (var pictureFile in files)
                {
                    Save(new PictureModel(pictureFile));
                }

                pictureModels = _pictureModels;
            }
            else
            {
                _pictureModels.Add(new PictureModel("Blume.jpg"));
                //Nach den Pictures suchen die den namen haben
                foreach (var pictureModel in _pictureModels)
                {
                    if (files.Contains(pictureModel.FileName)) continue;
                    files.Add(pictureModel.FileName);
                }
                //geh alle einträge durch und suche nach übereinstimmungen
                foreach (var pictureFile in files)
                {
                    if (pictureFile.ToUpper().Contains(namePart.ToUpper())) pictureModels.Add(new PictureModel(pictureFile));
                }
            }

            return pictureModels;
        }

        public IPictureModel GetPicture(int ID)
        {
            return new PictureModel("Testy.jpg");
        }

        public void Save(IPictureModel picture)
        {
            foreach (var pictureModel in _pictureModels)
            {
                if (pictureModel.FileName == picture.FileName) return;
            }

            _pictureModels.Add(picture);

            foreach (var pictureModel in _pictureModels)
            {
                pictureModel.ID = (int)Interlocked.Increment(ref _maxId);
            }
        }

        public void DeletePicture(int ID)
        {
            foreach (var picture in _pictureModels)
            {
                if (picture.ID != ID) continue;
                _pictureModels.Remove(picture);
                break;
            }
        }

        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            return _photographerModels;
        }

        public IPhotographerModel GetPhotographer(int ID)
        {
            return new PhotographerModel();
        }

        public void Save(IPhotographerModel photographer)
        {
            var maxPhotographerId = 1;
            if (_photographerModels.Contains(photographer)) return;

            _photographerModels.Add(photographer);

            foreach (var _photographer in _photographerModels)
            {
                _photographer.ID = maxPhotographerId;
                maxPhotographerId += 1;
            }
        }

        public void DeletePhotographer(int ID)
        {
            foreach (var photographer in _photographerModels)
            {
                if (photographer.ID != ID) continue;
                _photographerModels.Remove(photographer);
                break;

            }
        }

        public IEnumerable<ICameraModel> GetCameras()
        {
            return _cameraModels;
        }

        public ICameraModel GetCamera(int ID)
        {
            return new CameraModel();
        }
    }
}
