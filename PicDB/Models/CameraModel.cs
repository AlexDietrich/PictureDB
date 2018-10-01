using System;
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

        /// <summary>
        /// Database primary key
        /// </summary>
        public int ID { get; set; }

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
        /// Max ISO Value for good results. 0 means "not defined"
        /// </summary>
        public decimal ISOLimitGood { get; set; }

        /// <summary>
        /// Max ISO Value for acceptable results. 0 means "not defined"
        /// </summary>
        public decimal ISOLimitAcceptable { get; set; }
    }
}
