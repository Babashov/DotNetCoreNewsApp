using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsManagementDotNetCoreApp.Models
{
    public class SubCategory
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name="Sub Category Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

    }
}
