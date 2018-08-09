using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Camera = new CameraModel(mdl.Camera);
            Photographer = new PhotographerModel(mdl.Photographer);
        }

        public int ID { get; set; }
        public string FileName { get; set; }
        public IIPTCModel IPTC { get; set; }
        public IEXIFModel EXIF { get; set; }
        public ICameraModel Camera { get; set; }
        public IPhotographerModel Photographer { get; set; }
    }
}