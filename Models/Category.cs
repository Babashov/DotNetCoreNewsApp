using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsManagementDotNetCoreApp.Models
{
    public class Category
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name="Category Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Category Place")]
        public int CatPlace { get; set; }
    }
}
