﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FationShop.Areas.Admin.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Menu
    {
        public int ID { get; set; }
        [Display(Name = "Tên hiển thị")]
        [Required(ErrorMessage = "Tên hiển thị không được để trống")]
        public string Name { get; set; }
        [Display(Name = "Link")]
        [Required(ErrorMessage = "Link không được để trống")]
        public string Link { get; set; }
        [Display(Name = "Trạng thái hiển thị")]
        [Required(ErrorMessage = "Trạng thái hiển thị không được để trống")]
        public Nullable<bool> Status { get; set; }
        [Display(Name = "Thứ tự hiển thị")]
        [Required(ErrorMessage = "Thứ tự hiển thị không được để trống")]
        public Nullable<int> DisplayOrder { get; set; }
    }
}