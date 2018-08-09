using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.Models
{
    class IPTCModel : IIPTCModel
    {
        public IPTCModel() { }

        public IPTCModel(IIPTCViewModel iPTC)
        {
            this.ByLine = iPTC.ByLine;
            this.Keywords = iPTC.Keywords;
            this.Caption = iPTC.Caption;
            this.CopyrightNotice = iPTC.CopyrightNotice;
            this.Headline = iPTC.Headline;
        }

        public string Keywords { get; set; }
        public string ByLine { get; set; }
        public string CopyrightNotice { get; set; }
        public string Headline { get; set; }
        public string Caption { get; set; }
    }
}
