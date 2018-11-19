using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Models
{

    public class ListenContent : BaseContent
    {

    }

    public class Listen
    {
        public string _id { set; get; }
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
