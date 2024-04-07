using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYHF.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(1,1000)]
        public double Price { get; set; }
        public int Stock { get; set; }
        public int Ordered { get; set; }

        public string? ImageUrl { get; set; }
    }
}
