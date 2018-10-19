using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsManagementDotNetCoreApp.Models
{
    public class PostPlace
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name="Post Place Name")]
        public string Name { get; set; }

    }
}
