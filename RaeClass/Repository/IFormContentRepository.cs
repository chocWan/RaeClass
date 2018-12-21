using RaeClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Repository
{
    public interface IFormContentRepository
    {
        Task<int> AddAsync(FormContent baseFormContent);
        Task<int> UpdateAsync(FormContent baseFormContent);
        Tuple<List<FormContent>, int> GetPageListAsync(string contentType, string level, string titleOrContent, int pageindex, int pagesize);
        FormContent GetBaseFormContent(string fumber);
        FormContent GetEmptyFormContent();
    }
}
