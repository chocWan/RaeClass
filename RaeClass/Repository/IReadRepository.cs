using RaeClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Repository
{
    public interface IReadRepository
    {
        Task AddAsync(Read read);
        Task UpdateAsync(Read read);
        Tuple<List<ReadContent>, int> GetPageListAsync(int pageindex, int pagesize, string level, string titleOrContent);
    }
}
