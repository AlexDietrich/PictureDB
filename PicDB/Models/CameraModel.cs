using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.Models
{
    class CameraModel : ICameraModel
    {
        public CameraModel() { }

        public CameraModel(string producer, string make)
        {
            Producer = producer;
            Make = make;
        }

        public CameraModel(ICameraViewModel camera)
        {
            if (camera == null) return;
            this.ID = camera.ID;
            this.BoughtOn = camera.BoughtOn;
            this.ISOLimitAcceptable = camera.ISOLimitAcceptable;
            this.ISOLimitGood = camera.ISOLimitGood;
            this.Make = camera.Make;
            this.Notes = camera.Notes;
            this.Producer = camera.Producer;
        }

        public int ID { get; set; }
        public string Producer { get; set; }
        public string Make { get; set; }
        public DateTime? BoughtOn { get; set; }
        public string Notes { get; set; }
        public decimal ISOLimitGood { get; set; }
        public decimal ISOLimitAcceptable { get; set; }
    }
}
