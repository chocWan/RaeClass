using RaeClass.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaeClass.Helper
{
    public static class CommonUtils
    {

        public static string GetDateTimeNowSerial()
        {
            //日期流水：20180909
            var dt = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.Year.ToString());
            sb.Append(dt.Month > 10 ? dt.Month.ToString() : "0" + dt.Month);
            sb.Append(dt.Day > 10 ? dt.Day.ToString() : "0" + dt.Day);
            return sb.ToString();
        }

        public static string GetRecordFilePrefix(RaeClassContentType contentType)
        {
            switch (contentType)
            {
                case RaeClassContentType.Read:return CONST.WX_READ_RECORD_PREFIX;
                case RaeClassContentType.Listen:return CONST.WX_LISTEN_RECORD_PREFIX;
                case RaeClassContentType.Spoken:return CONST.WX_SPOKEN_RECORD_PREFIX;
            }
            return string.Empty;
        }

    }
}
