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
        public string Account { get; set; }

        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }

    public class AuthUser
    {
        public string Account { get; set; }

        public bool IsAuth { get; set; }
    }



}
