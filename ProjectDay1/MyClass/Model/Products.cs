using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Products")]
    public class Products
    {
        [Key]
        public int Id { get; set; }

        public int CatID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Supplier { get; set; }

        public string Slug { get; set; }

        [Required]
        public string Detail { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public decimal PriceSale { get; set; }

        public int Amount { get; set; }

        [Required]
        public string MetaDesc { get; set; }

        [Required]
        public string MetaKey { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UpdateBy { get; set; }

        public DateTime UpdateAt { get; set; }

        public int Status { get; set; }
    }
}
