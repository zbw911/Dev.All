using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASServer.Models
{
    public class GetPwdModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "验证码")]
        [Required(ErrorMessage = "验证码是必须的")]
        [StringLength(4, ErrorMessage = "{0}最大长度为{1}")]
        public string Validcode { get; set; }

        [DefaultValue(0)]
        public int GetPwdType { get; set; }
    }
}