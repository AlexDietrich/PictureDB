using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.Models
{
    class PhotographerModel : IPhotographerModel
    {
        public PhotographerModel() { }

        public PhotographerModel(IPhotographerViewModel photographer)
        {
            if (photographer == null) return;
            this.ID = photographer.ID;
            this.BirthDay = photographer.BirthDay;
            this.FirstName = photographer.FirstName;
            this.LastName = photographer.LastName;
            this.Notes = photographer.Notes;
        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Notes { get; set; }
    }
}
