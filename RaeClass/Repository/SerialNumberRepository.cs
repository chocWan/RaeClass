using RaeClass.Config;
using RaeClass.Helper;
using RaeClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Repository
{
    public class SerialNumberRepository : ISerialNumberRepository
    {
        private RaeClassContext raeClassContext;
        public SerialNumberRepository(RaeClassContext _raeClassContext)
        {
            raeClassContext = _raeClassContext;
        }

        public string GetSerialNumber(RaeClassContentType contentType)
        {
            int maxIndex = GetMaxIndex(contentType) + 1;
            return contentType.ToString() + CommonUtils.GetDateTimeNowSerial() +maxIndex.ToString();
        }

        public int GetMaxIndex(RaeClassContentType contentType)
        {
            return raeClassContext.SerialNumberSet.Where(x=>x.FContentType.Equals(contentType.ToString())).FirstOrDefault().FCurrentGeneratedIndex;
        }

        public void UpdateMaxIndex(RaeClassContentType contentType,int gap)
        {
            var query = raeClassContext.SerialNumberSet.Where(x => x.FContentType.Equals(contentType.ToString())).FirstOrDefault();
            query.FCurrentGeneratedIndex += gap;
            raeClassContext.SaveChanges();
        }
    }
}
