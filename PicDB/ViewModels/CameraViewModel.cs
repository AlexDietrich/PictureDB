using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.ViewModels;
using BIF.SWE2.Interfaces.Models;
using PicDB.Models;

namespace PicDB.ViewModels
{
    class CameraViewModel : ICameraViewModel
    {
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

        public int ID { get; }

        public string Producer { get; set; }

        public string Make { get; set; }

        public DateTime? BoughtOn { get; set; }

        public string Notes { get; set; }

        public int NumberOfPictures { get; }

        public bool IsValid => IsValidBoughtOn && IsValidMake && IsValidProducer;

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

        public bool IsValidProducer => !string.IsNullOrEmpty(Producer) && !string.IsNullOrWhiteSpace(Producer);

        public bool IsValidMake => !string.IsNullOrEmpty(Make) && !string.IsNullOrWhiteSpace(Make);

        public bool IsValidBoughtOn
        {
            get
            {
                if (BoughtOn == null) return true;
                return BoughtOn < DateTime.Now;
            }
        }

        public decimal ISOLimitGood { get; set; }

        public decimal ISOLimitAcceptable { get; set; }
    }
}
