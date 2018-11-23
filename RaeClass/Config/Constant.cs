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
    }

    public static class Const
    {
        public static string WX_OPENID = "oT_iO4tBijzyXT91iOW6wmGF801Q";
        public static string WX_READ_RECORD_PREFIX = "cloud://reaclass-dev-b66f08.7265-reaclass-dev-b66f08/";
    }

}
