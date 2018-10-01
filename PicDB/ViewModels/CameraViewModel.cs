using System;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.ViewModels;
using BIF.SWE2.Interfaces.Models;

namespace PicDB.ViewModels
{
    class CameraViewModel : ICameraViewModel
    {
        /// <summary>
        /// Translates a given ISO value to a ISO rating
        /// </summary>
        /// <param name="iso"></param>
        /// <returns></returns>
        public ISORatings TranslateISORating(decimal iso)
        {
            if (iso == 0) return ISORatings.NotDefined;
            if (iso <= ISOLimitGood) return ISORatings.Good;
            if (iso <= ISOLimitAcceptable) return ISORatings.Acceptable;
            if (iso > ISOLimitAcceptable) return ISORatings.Noisey;
            throw new ArgumentOutOfRangeException();
        }
        
        public CameraViewModel() { }

        public CameraViewModel(ICameraModel mdl)
        {
            if (mdl == null) return; 
            this.Producer = mdl.Producer;
            this.BoughtOn = mdl.BoughtOn;
            this.ID = mdl.ID;
            this.ISOLimitAcceptable = mdl.ISOLimitAcceptable;
            this.ISOLimitGood = mdl.ISOLimitGood;
            this.Make = mdl.Make;
            this.Notes = mdl.Notes;
        }

        /// <summary>
        /// Database primary key
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Name of the producer
        /// </summary>
        public string Producer { get; set; }

        /// <summary>
        /// Name of camera
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// Optional: date, when the camera was bought
        /// </summary>
        public DateTime? BoughtOn { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Returns the number of Pictures
        /// </summary>
        public int NumberOfPictures { get; }

        /// <summary>
        /// Returns true, if the model is valid
        /// </summary>
        public bool IsValid => IsValidBoughtOn && IsValidMake && IsValidProducer;

        /// <summary>
        /// Returns a summary of validation errors
        /// </summary>
        public string ValidationSummary
        {
            get
            {
                var summary = string.Empty;
                if (IsValid) return null;
                //Wenn BoughtOn Value nicht valid ist dann gib einen Fehler zurück
                summary += (!IsValidBoughtOn) ? "BoughtOn value isn't valid" : string.Empty;
                //Falls bereits ein Fehler registriert ist, füge ein Trennzeichen hinzu
                summary += (!string.IsNullOrEmpty(summary)) ? "/" : string.Empty;
                //Wenn der Make Value nicht valid ist dann gibt einen Fehler zurück
                summary += (!IsValidMake) ? "Make value isn't valid" : string.Empty;
                summary += (!string.IsNullOrEmpty(summary)) ? "/" : string.Empty;
                //Wenn der Producer Value nicht valid ist dann gibt einen Fehler zurück
                summary += (!IsValidProducer) ? "Producer Value isn't valid" : string.Empty;
                return summary;
            }
        }

        /// <summary>
        /// returns true if the producer name is valid
        /// </summary>
        public bool IsValidProducer => !string.IsNullOrEmpty(Producer) && !string.IsNullOrWhiteSpace(Producer);

        /// <summary>
        /// returns true if the make is valid
        /// </summary>
        public bool IsValidMake => !string.IsNullOrEmpty(Make) && !string.IsNullOrWhiteSpace(Make);

        /// <summary>
        /// returns true if the "bought on" date is valid
        /// </summary>
        public bool IsValidBoughtOn
        {
            get
            {
                if (BoughtOn == null) return true;
                return BoughtOn < DateTime.Now;
            }
        }

        /// <summary>
        /// Max ISO Value for good results. 0 means "not defined"
        /// </summary>
        public decimal ISOLimitGood { get; set; }

        /// <summary>
        /// Max ISO Value for acceptable results. 0 means "not defined"
        /// </summary>
        public decimal ISOLimitAcceptable { get; set; }
    }
}
