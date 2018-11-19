using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RaeClass.Models;
using Microsoft.EntityFrameworkCore;

namespace RaeClass.Repository
{
    public class ReadRepository : IReadRepository
    {
        private RaeClassContext context;
        public ReadRepository(RaeClassContext _context)
        {
            this.context = _context;
        }
        public Task AddAsync(ReadContent content)
        {
            context.ReadContentSet.Add(content);
            return context.SaveChangesAsync();
        }

        public Task<List<ReadContent>> ListAsync()
        {
            var query = context.ReadContentSet.AsQueryable();
            return query.ToListAsync();
        }

        public Tuple<List<ReadContent>, int> PageListAsync(int pageindex, int pagesize)
        {
            var query = context.ReadContentSet.AsQueryable();
            var count = query.Count();
            var pagecount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            var contents = query.OrderByDescending(r => r.FCreateTime)
                .Skip((pageindex - 1) * pagesize)
                .Take(pagesize)
                .ToList();
            return new Tuple<List<ReadContent>, int>(contents, pagecount);
        }

        public Task UpdateAsync(ReadContent content)
        {
            context.Entry(content).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return context.SaveChangesAsync();
        }
    }
}
