using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Models
{
    public class BaseFormContent
    {
        [Key]
        public int FId { set; get; }
        public string FContentType { set; get; }
        public string FDocStatus { set; get; }
        public string FNumber { set; get; }
        public string FName { set; get; }
        public string FLevel { set; get; }
        public string FJsonData { set; get; }
        public DateTime FCreateTime { set; get; }
        public DateTime FModifyTime { set; get; }
    }

    public class FormContent
    {
        public string _openid { set; get; }
        public string flevel { set; get; }
        public string fnumber { set; get; }
        public string fname { set; get; }
        public string fcnContent { set; get; }
        public string fenContent { set; get; }
        public string fcreateTime { set; get; }
        public string fcreateBy { set; get; }
        public string fmodifyTime { set; get; }
        public string fmodifyBy { set; get; }
        public string fdocStatus { set; get; }
        public string frecordFileId1 { set; get; }
        public string frecordFileId2 { set; get; }
    }


}
