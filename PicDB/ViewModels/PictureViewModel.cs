using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB.Models;

namespace PicDB.ViewModels
{
    class PictureViewModel : IPictureViewModel
    {
        public PictureViewModel() { }

        public PictureViewModel(IPictureModel mdl)
        {
            if (mdl == null) return;
            var mdlCast = (PictureModel) mdl; //Wird gebraucht für Photographer 
            this.ID = mdl.ID;
            this.FileName = mdl.FileName;
            this.EXIF = new EXIFViewModel(mdl.EXIF);
            this.IPTC = new IPTCViewModel(mdl.IPTC);
            this.Camera = new CameraViewModel(mdl.Camera);
            this.Photographer = new PhotographerViewModel(mdlCast.Photographer);
            this.FilePath = GlobalInformation.Path + "\\" + FileName;
        }

        public int ID { get; }

        public string FileName { get; }

        public string FilePath { get; }

        public string DisplayName
        {
            get
            {
                var displayName = string.Empty;
                if (string.IsNullOrEmpty(IPTC.Headline)) displayName += FileName;
                else displayName += IPTC.Headline;

                displayName += " (by ";

                if (!string.IsNullOrEmpty(Photographer?.FirstName)) displayName += Photographer.FirstName;
                if (!string.IsNullOrEmpty(Photographer?.LastName)) displayName += Photographer.LastName;
                else if(string.IsNullOrEmpty(IPTC?.ByLine)) displayName += IPTC.ByLine;
                displayName += ")";

                return displayName;
            }
        }

        public IIPTCViewModel IPTC { get; }

        public IEXIFViewModel EXIF { get; }

        public IPhotographerViewModel Photographer { get; set; }

        public ICameraViewModel Camera { get; set; }
    }
}
