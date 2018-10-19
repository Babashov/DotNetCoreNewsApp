using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsManagementDotNetCoreApp.Models.PostViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }


        public IEnumerable<Category> Category { get; set; }

        public IEnumerable<SubCategory> SubCategory { get; set; }

        public IEnumerable<PostPlace> PostPlace { get; set; }
    }
}
