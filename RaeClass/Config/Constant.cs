using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Config
{

    public enum RaeClassContentType : int
    {
        Read = 1,
        Listen = 2,
        Spoken = 3,
        Tech = 4
    }

    public static class DocStatus
    {
        public static readonly string SAVE = "A";
        public static readonly string AUDIT = "C";
        public static readonly string DELETE = "D";
        public static readonly string FORBID = "B";
    }

    public static class CONST
    {
        public static readonly string WX_OPENID = "oT_iO4tBijzyXT91iOW6wmGF801Q";
        public static readonly string WX_READ_RECORD_PREFIX = "cloud://reaclass-dev-b66f08.7265-reaclass-dev-b66f08/read/";
        public static readonly string WX_LISTEN_RECORD_PREFIX = "cloud://reaclass-dev-b66f08.7265-reaclass-dev-b66f08/listen/";
        public static readonly string WX_SPOKEN_RECORD_PREFIX = "cloud://reaclass-dev-b66f08.7265-reaclass-dev-b66f08/spoken/";
        public static readonly string CREATOR = "Rae";
             
    }

}
