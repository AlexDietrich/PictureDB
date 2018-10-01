using System;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.ViewModels
{
    public class EXIFViewModel : IEXIFViewModel
    {
        public EXIFViewModel() { }

        public EXIFViewModel(IEXIFModel mdl)
        {
            this.Model = mdl ?? throw new ArgumentNullException();
            this.Make = mdl.Make;
            this.ExposureProgram = Enum.GetName(typeof(ExposurePrograms), mdl.ExposureProgram);
            this.FNumber = mdl.FNumber;
            this.ExposureTime = mdl.ExposureTime;
            this.Flash = mdl.Flash;
            this.ISOValue = mdl.ISOValue;
        }
        
        public IEXIFModel Model { get; set; }

        /// <summary>
        /// Name of camera
        /// </summary>
        public string Make { get; }

        /// <summary>
        /// Aperture number
        /// </summary>
        public decimal FNumber { get; }

        /// <summary>
        /// Exposure time
        /// </summary>
        public decimal ExposureTime { get; }

        /// <summary>
        /// ISO value
        /// </summary>
        public decimal ISOValue { get; }

        /// <summary>
        /// Flash yes/no
        /// </summary>
        public bool Flash { get; }

        /// <summary>
        /// The Exposure Program as string
        /// </summary>
        public string ExposureProgram { get; }

        /// <summary>
        /// The Exposure Program as image resource
        /// </summary>
        public string ExposureProgramResource => ExposureProgram;

        /// <summary>
        /// Gets or sets a optional camera view model
        /// </summary>
        public ICameraViewModel Camera { get; set; }

        /// <summary>
        /// Returns a ISO rating if a camera is set.
        /// </summary>
        public ISORatings ISORating => Camera?.TranslateISORating(ISOValue) ?? ISORatings.NotDefined;

        /// <summary>
        /// Returns a image resource of a ISO rating if a camera is set.
        /// </summary>
        public string ISORatingResource => ISORating.ToString();
    }
}
