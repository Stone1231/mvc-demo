using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace WebApplication1
{
    public class LoginModels
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "使用者帳號")]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Pwd { get; set; }
    }
}