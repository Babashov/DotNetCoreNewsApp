using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsManagementDotNetCoreApp.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Post> Post { get; set; }

        public IEnumerable<Category> Category { get; set; }

        public IEnumerable<PostPlace> PostPlace { get; set; }

        public string StatusMessage { get; set; }
    }
}
