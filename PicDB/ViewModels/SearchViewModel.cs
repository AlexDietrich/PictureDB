using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.ViewModels
{
    class SearchViewModel : ISearchViewModel
    {
        /// <summary>
        /// The search text
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// True, if a search is active
        /// </summary>
        public bool IsActive => !string.IsNullOrEmpty(SearchText);

        /// <summary>
        /// Number of photos found.
        /// </summary>
        public int ResultCount { get; }
    }
}
