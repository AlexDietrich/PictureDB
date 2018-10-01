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

        /// <summary>
        /// A list of keywords
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Name of the photographer
        /// </summary>
        public string ByLine { get; set; }

        /// <summary>
        /// copyright noties. 
        /// </summary>
        public string CopyrightNotice { get; set; }

        /// <summary>
        /// A list of common copyright noties. e.g. All rights reserved, CC-BY, CC-BY-SA, CC-BY-ND, CC-BY-NC, CC-BY-NC-SA, CC-BY-NC-ND
        /// </summary>
        public IEnumerable<string> CopyrightNotices { get; } = _copyrightNotices;
        
        /// <summary>
        /// Summary/Headline of the picture
        /// </summary>
        public string Headline { get; set; }

        /// <summary>
        /// Caption/Abstract, a description of the picture
        /// </summary>
        public string Caption { get; set; }
    }
}
