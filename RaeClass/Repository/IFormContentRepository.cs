using RaeClass.Config;
using RaeClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Repository
{
    public interface IFormContentRepository
    {
        Task<FormContent> AddAsync(RaeClassContentType contentType, FormContent formContent);
        Task<int> AddListAsync(RaeClassContentType contentType, List<FormContent> formContent);
        Task<FormContent> UpdateAsync(FormContent formContent);
        Task<int> UpdateListAsync(List<FormContent> formContents);
        Task<int> UpdateDocStatusListAsync(List<string> fnumbers, string docStatus);
        Tuple<List<FormContent>, int> GetPageListAsync(RaeClassContentType raeClassContentType, string level, string titleOrContent, int pageindex, int pagesize);
        Task<FormContent> GetFormContentAsync(string fumber);
        Task<List<FormContent>> GetFormContentListAsync(List<string> fumbers);
        Task<FormContent> GetEmptyFormContent();
        List<ArticleGroupModel> GetArticlesQtyByDate(DateTime sDateTime, DateTime eDateTime);
    }
}
