using System;
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

        /// <summary>
        /// Database primary key
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Firstname, including middle name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Lastname
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Birthday
        /// </summary>
        public DateTime? BirthDay { get; set; }

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
        public bool IsValid => IsValidBirthDay && IsValidLastName;

        /// <summary>
        /// Returns a summary of validation errors
        /// </summary>
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

        /// <summary>
        /// returns true if the last name is valid
        /// </summary>
        public bool IsValidLastName => !string.IsNullOrEmpty(LastName) && !string.IsNullOrWhiteSpace(LastName);
        
        /// <summary>
        /// returns true if the birthday is valid
        /// </summary>
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
