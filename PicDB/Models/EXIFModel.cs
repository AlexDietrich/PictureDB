using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.Models
{
    class EXIFModel : IEXIFModel
    {
        public EXIFModel() { }

        public EXIFModel(IEXIFViewModel eXIF)
        {
            this.Make = eXIF.Make;
            this.ExposureProgram = ConvertExposureProgram(eXIF.ExposureProgram);
            this.ExposureTime = eXIF.ExposureTime;
            this.FNumber = eXIF.FNumber;
            this.ISOValue = eXIF.ISOValue;
            this.Flash = eXIF.Flash;
        }

        public string Make { get; set; }
        public decimal FNumber { get; set; }
        public decimal ExposureTime { get; set; }
        public decimal ISOValue { get; set; }
        public bool Flash { get; set; }
        public ExposurePrograms ExposureProgram { get; set; }


        private ExposurePrograms ConvertExposureProgram(string ExposureProgramAsString)
        {
            switch (ExposureProgramAsString)
            {
                case "Manual":
                    return ExposurePrograms.Manual;
                case "Normal":
                    return ExposurePrograms.Normal;
                case "AperturePriority":
                    return ExposurePrograms.AperturePriority;
                case "ShutterPriority":
                    return ExposurePrograms.ShutterPriority;
                case "CreativeProgram":
                    return ExposurePrograms.CreativeProgram;
                case "ActionProgram":
                    return ExposurePrograms.ActionProgram;
                case "PortraitMode":
                    return ExposurePrograms.PortraitMode;
                case "LandscapeMode":
                    return ExposurePrograms.LandscapeMode;
                default:
                    return ExposurePrograms.NotDefined;
            }
        }
    }
}
