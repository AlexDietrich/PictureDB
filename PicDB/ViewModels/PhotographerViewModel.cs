using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.ViewModels
{
    public class PhotographerViewModel : IPhotographerViewModel
    {
        public PhotographerViewModel() { }
        public PhotographerViewModel(IPhotographerModel photographer)
        {
            if (photographer == null) return;
            this.ID = photographer.ID;
            this.BirthDay = photographer.BirthDay;
            this.FirstName = photographer.FirstName;
            this.LastName = photographer.LastName;
            this.Notes = photographer.Notes;

        }
        public int ID { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Notes { get; set; }
        public int NumberOfPictures { get; }
        public bool IsValid => IsValidBirthDay && IsValidLastName;

        public string ValidationSummary
        {
            get
            {
                var summary = string.Empty;
                if (IsValid) return null;
                //Wenn Birthday Value nicht valid ist dann gib einen Fehler zurück
                summary += (!IsValidBirthDay) ? "Birthday Value isn't valid" : string.Empty;
                //Falls bereits ein Fehler registriert ist, füge ein Trennzeichen hinzu
                summary += (!string.IsNullOrEmpty(summary)) ? "/" : string.Empty;
                //Wenn Lastname Value nicht valid ist dann gib einen Fehler zurück
                summary += (!IsValidLastName) ? "Lastname Value isn't valid" : string.Empty;
                return summary;
            }
        }

        public bool IsValidLastName => !string.IsNullOrEmpty(LastName) && !string.IsNullOrWhiteSpace(LastName);
        public bool IsValidBirthDay
        {
            get
            {
                if (BirthDay == null) return true;
                return BirthDay < DateTime.Now;
            }
        }
    }
}
