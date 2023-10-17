using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Categories")]
    public class Categories
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="tên loại SP không để trống")]
        [Display(Name="Tên loại SP")]
        public string Name { get; set; }

        [Display(Name="Tên rút gọn")]
        public string Slug { get; set; }

        [Display(Name = "Cấp cha")]
        public int? ParentId { get; set; }

        [Display(Name = "Sắp xếp")]
        public int? Order { get; set; }

        [Display(Name = "Mô tả")]

        [Required(ErrorMessage = "Mô tả không để trống")]
        public string MetaDesc { get; set; }

        [Display(Name = "Từ khoá")]

        [Required(ErrorMessage = "Từ khoá không để trống")]
        public string MetaKey { get; set; }

        [Display(Name = "Tạo bởi")]
        [Required(ErrorMessage = "Người tạo không để trống")]
        public int CreatedBy { get; set; }

        [Display(Name = "Ngày tạo")]
        [Required(ErrorMessage = "Ngày tạo không để trống")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Cập nhật bởi")]
        [Required(ErrorMessage = "Người cập nhật không để trống")]
        public int UpdateBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        [Required(ErrorMessage = "Ngày cập nhật không để trống")]
        public DateTime UpdateAt { get; set; }

        [Display(Name = "Trạng thái")]
        [Required(ErrorMessage = "Trạng thái không để trống")]
        public int Status { get; set; }
    }
}
