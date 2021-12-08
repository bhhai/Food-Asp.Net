using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FationShop.Areas.Admin.Framework
{
    public class AboutMetadata
    {
        public int ID { get; set; }
        [Display(Name = "Link")]
        [Required(ErrorMessage = "Link không được để trống!")]
        public string MetaTitle { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Nội dung")]
        [Required(ErrorMessage = "Nội dung không được để trống!")]
        public string Content { get; set; }
        [Display(Name = "Ảnh đại diện")]
        [Required(ErrorMessage = "Ảnh đại diện không được để trống!")]
        public string Avartar { get; set; }
        [Display(Name = "Trạng thái hiển thị")]
        public Nullable<bool> Status { get; set; }
    }
}