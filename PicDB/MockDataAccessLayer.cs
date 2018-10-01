using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// Returns a filterd list of Pictures from the directory, based on a database query.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns ONE Picture from the database.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IPictureModel GetPicture(int ID)
        {
            return new PictureModel("Testy.jpg");
        }

        /// <summary>
        /// Saves all changes to the database.
        /// </summary>
        /// <param name="picture"></param>
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

        /// <summary>
        /// Deletes a Picture from the database.
        /// </summary>
        /// <param name="ID"></param>
        public void DeletePicture(int ID)
        {
            foreach (var picture in _pictureModels)
            {
                if (picture.ID != ID) continue;
                _pictureModels.Remove(picture);
                break;
            }
        }

        /// <summary>
        /// Returns a list of ALL Photographers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            return _photographerModels;
        }

        /// <summary>
        /// Returns ONE Photographer
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IPhotographerModel GetPhotographer(int ID)
        {
            return new PhotographerModel();
        }

        /// <summary>
        /// Saves all changes.
        /// </summary>
        /// <param name="photographer"></param>
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

        /// <summary>
        /// Deletes a Photographer. A Exception is thrown if a Photographer is still linked to a picture.
        /// </summary>
        /// <param name="ID"></param>
        public void DeletePhotographer(int ID)
        {
            foreach (var photographer in _photographerModels)
            {
                if (photographer.ID != ID) continue;
                _photographerModels.Remove(photographer);
                break;

            }
        }

        /// <summary>
        /// Returns a list of ALL Cameras.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ICameraModel> GetCameras()
        {
            return _cameraModels;
        }

        /// <summary>
        /// Returns ONE Camera
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ICameraModel GetCamera(int ID)
        {
            return new CameraModel();
        }
    }
}
