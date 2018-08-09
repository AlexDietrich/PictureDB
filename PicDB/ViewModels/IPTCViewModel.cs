using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.ViewModels
{
    class IPTCViewModel : IIPTCViewModel
    {
        private static readonly IEnumerable<string> _copyrightNotices = new List<string>()
        {
            "All rights reserved",
            "CC - BY",
            "CC - BY - SA",
            "CC - BY - ND",
            "CC - BY - NC",
            "CC - BY - NC - SA",
            "CC - BY - NC - ND",
        };

        public IPTCViewModel(IIPTCModel mdlIptc)
        {
            if (mdlIptc == null) return;
            this.Keywords = mdlIptc.Keywords;
            this.ByLine = mdlIptc.ByLine;
            this.Caption = mdlIptc.Caption;
            this.CopyrightNotice = mdlIptc.CopyrightNotice;
            this.Headline = mdlIptc.Headline;
        }

        public IPTCViewModel() { }

        public string Keywords { get; set; }
        public string ByLine { get; set; }
        public string CopyrightNotice { get; set; }
        public IEnumerable<string> CopyrightNotices { get; } = _copyrightNotices;
        public string Headline { get; set; }
        public string Caption { get; set; }
    }
}
