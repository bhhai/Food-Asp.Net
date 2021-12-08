using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FationShop.Areas.Admin.Models.DTO
{
    public class CategoryMetadata
    {
        [Display(Name = "Mã danh mục")]
        public int ID { get; set; }
        [Display(Name = "Tên danh mục")]
        [Required(ErrorMessage = "Tên danh mục không được để trống!")]
        public string Name { get; set; }
        [Display(Name = "Tiêu đề SEO")]
        [Required(ErrorMessage = "iêu đề SEO không được để trống!")]
        public string MetaTitle { get; set; }
        [Display(Name = "Thứ tự hiển thị")]
        [Required(ErrorMessage = "Thứ tự hiển thị không được để trống!")]
        public Nullable<int> DisplayOrder { get; set; }
        [Display(Name = "Trạng thái hiển thị")]
        public Nullable<bool> Status { get; set; }

    }
}