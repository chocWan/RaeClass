using RaeClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Repository
{
    public interface IReadRepository
    {
        Task AddAsync(ReadContent content);
        Task UpdateAsync(ReadContent content);
        Task<List<ReadContent>> ListAsync();
        Tuple<List<ReadContent>, int> PageListAsync(int pageindex, int pagesize);
    }
}
