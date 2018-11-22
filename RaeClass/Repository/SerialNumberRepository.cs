using RaeClass.Config;
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
