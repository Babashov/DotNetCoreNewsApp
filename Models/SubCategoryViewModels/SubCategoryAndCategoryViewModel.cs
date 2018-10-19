using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsManagementDotNetCoreApp.Models.SubCategoryViewModels
{
    public class SubCategoryAndCategoryViewModel
    {
        public SubCategory SubCategory { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }

        public List<string> SubCategoryList { get; set; }

        public string StatusMessage { get; set; }
    }
}

