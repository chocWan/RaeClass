using RaeClass.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Repository
{
    public interface ISerialNumberRepository
    {
        int GetMaxIndex(RaeClassContentType raeClassContentType);
        void UpdateMaxIndex(RaeClassContentType raeClassContentType,int gap = 1);
        string GetSerialNumber(RaeClassContentType contentType);
    }
}
