using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.ViewModels
{
    class EXIFViewModel : IEXIFViewModel
    {
        public EXIFViewModel() { }

        public EXIFViewModel(IEXIFModel mdl)
        {
            if (mdl == null) return;
            this.Make = mdl.Make;
            this.ExposureProgram = Enum.GetName(typeof(ExposurePrograms), mdl.ExposureProgram);
            this.FNumber = mdl.FNumber;
            this.ExposureTime = mdl.ExposureTime;
            this.Flash = mdl.Flash;
            this.ISOValue = mdl.ISOValue;
        }

        public string Make { get; }
        public decimal FNumber { get; }
        public decimal ExposureTime { get; }
        public decimal ISOValue { get; }
        public bool Flash { get; }
        public string ExposureProgram { get; }
        public string ExposureProgramResource => ExposureProgram;
        public ICameraViewModel Camera { get; set; }
        public ISORatings ISORating => Camera?.TranslateISORating(ISOValue) ?? ISORatings.NotDefined;
        public string ISORatingResource => ISORating.ToString();
    }
}
