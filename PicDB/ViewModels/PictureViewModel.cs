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
            this.EXIF.Camera = this.Camera; 
        }

        /// <summary>
        /// Database primary key
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Name of the file
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Full file path, used to load the image
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// The line below the Picture. Format: {IPTC.Headline|FileName} (by {Photographer|IPTC.ByLine}).
        /// </summary>
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

        /// <summary>
        /// The IPTC ViewModel
        /// </summary>
        public IIPTCViewModel IPTC { get; }

        /// <summary>
        /// The EXIF ViewModel
        /// </summary>
        public IEXIFViewModel EXIF { get; }

        /// <summary>
        /// The Photographer ViewModel
        /// </summary>
        public IPhotographerViewModel Photographer { get; set; }

        /// <summary>
        /// The Camera ViewModel
        /// </summary>
        public ICameraViewModel Camera { get; set; }
    }
}
