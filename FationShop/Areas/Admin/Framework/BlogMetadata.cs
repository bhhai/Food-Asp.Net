using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FationShop.Areas.Admin.Models.DTO
{
    public class BlogMetadata
    {
        [Display(Name = "Mã blog")]
        public int ID { get; set; }
        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Tiêu đề không được bỏ trống!")]
        public string Name { get; set; }
        [Display(Name = "Link")]
        [Required(ErrorMessage = "Link không được bỏ trống!")]
        public string MetaTitle { get; set; }
        [Display(Name = "Mô tả")]
        public string SubTitle { get; set; }
        public string Description { get; set; }
        [Display(Name = "Nội dung blog")]
        [Required(ErrorMessage = "Nội dung blog không được bỏ trống!")]
        public string Content { get; set; }
        [Display(Name = "Ảnh đại diện")]
        [Required(ErrorMessage = "Ảnh đại diện không được bỏ trống!")]
        public string Avartar { get; set; }
        [Display(Name = "Ảnh mô tả")]
        public string Images { get; set; }
        [Display(Name = "Trạng thái hiển thị")]
        public Nullable<bool> Status { get; set; }
        public string Tags { get; set; }
    }
}