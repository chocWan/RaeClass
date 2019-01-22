using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Models
{
    public class User
    {
        [Key]
        public int Fid { set; get; }
        public string FUserName { set; get; }
        public string FPassWord { set; get; }
        public DateTime FCreateDate { set; get; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "用户名不能为空。")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空。")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }



}
