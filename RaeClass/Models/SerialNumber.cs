using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Models
{
    public class SerialNumber
    {
        [Key]
        public int FID { set; get;}
        public string FContentType { set; get; }
        public int FCurrentGeneratedIndex { set; get; }
        public DateTime FModifyTime { set; get; }
    }
}
