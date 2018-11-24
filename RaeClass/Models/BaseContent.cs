using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Models
{
    public class BaseContent
    {
        [Key]
        public int FId { set; get; }
        public string FNumber { set; get; }
        public string FName { set; get; }
        public string FLevel { set; get; }
        public string FJsonData { set; get; }
        public DateTime FCreateTime { set; get; }
        public DateTime FModifyTime { set; get; }
    }
}
