using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.ViewModels;

namespace PicDB.ViewModels
{
    class SearchViewModel : ISearchViewModel
    {
        public string SearchText { get; set; }

        public bool IsActive => !string.IsNullOrEmpty(SearchText);

        public int ResultCount { get; }
    }
}
