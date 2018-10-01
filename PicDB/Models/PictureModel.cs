using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.Models
{
    class PictureModel : IPictureModel
    {
        public PictureModel()
        {
        }

        public PictureModel(string filename)
        {
            FileName = filename;
        }

        public PictureModel(IPictureViewModel mdl)
        {
            ID = mdl.ID;
            FileName = mdl.FileName;
            IPTC = new IPTCModel(mdl.IPTC); 
            EXIF = new EXIFModel(mdl.EXIF);
            if(mdl.Camera != null) Camera = new CameraModel(mdl.Camera);
            if(mdl.Photographer != null) Photographer = new PhotographerModel(mdl.Photographer);
        }

        /// <summary>
        /// Database primary key
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Filename of the picture, without path.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// IPTC information
        /// </summary>
        public IIPTCModel IPTC { get; set; }

        /// <summary>
        /// EXIF information
        /// </summary>
        public IEXIFModel EXIF { get; set; }

        /// <summary>
        /// The camera (optional)
        /// </summary>
        public ICameraModel Camera { get; set; }
        public IPhotographerModel Photographer { get; set; }
    }
}