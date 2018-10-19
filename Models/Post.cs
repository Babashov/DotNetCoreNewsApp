using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsManagementDotNetCoreApp.Models
{
    public class Post
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name="Post Name")]
        public string Name { get; set; }

        [Display(Name="Post Description")]
        public string Desc { get; set; }

        [Display(Name="Post Image")]
        public string Image { get; set; }

        [Display(Name="Video")]
        public string Video { get; set; }

        [Display(Name="Post Date")]
        public DateTime Date { get; set; }

        [Display(Name="Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name="Sub Category")]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }

        [Display(Name="Post Place")]
        public int PostPlaceId { get; set; }

        [ForeignKey("PostPlaceId")]
        public virtual PostPlace PostPlace { get; set; }
    }
}
