using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RaeClass.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using RaeClass.Helper;

namespace RaeClass.Repository
{
    public class ReadRepository : IReadRepository
    {
        private RaeClassContext context;
        public ReadRepository(RaeClassContext _context)
        {
            this.context = _context;
        }

        public Task AddAsync(Read read)
        {
            ReadContent content = GetReadContent(read);
            context.ReadContentSet.Add(content);
            return context.SaveChangesAsync();
        }

        public Tuple<List<ReadContent>, int> GetPageListAsync(int pageindex, int pagesize,string level,string titleOrContent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select JsonData from ReadContent where 1=1");
            if (level.Trim() != "0")
            {
                sb.AppendFormat(" and json_extract(JsonData,\"$.flevel\") = '{0}' ",level);
            }
            if (!string.IsNullOrEmpty(titleOrContent))
            {
                sb.Append(" and (");
                sb.AppendFormat(" json_extract(JsonData,\"$.fname\") = '{0}' ", titleOrContent);
                sb.Append(" or");
                sb.AppendFormat(" json_extract(JsonData,\"$.fcnContent\") = '{0}' ", titleOrContent);
                sb.Append(" or");
                sb.AppendFormat(" json_extract(JsonData,\"$.fenContent\") = '{0}' ", titleOrContent);
                sb.Append(" )");
            }
            int count = context.ReadContentSet.FromSql(sb.ToString()).Count();
            sb.Append(" limit {0} offset {1}",pagesize,pageindex);
            var contents = context.ReadContentSet.FromSql(sb.ToString()).ToList();
            var pagecount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            
            return new Tuple<List<ReadContent>, int>(contents, pagecount);
        }

        public Task UpdateAsync(Read read)
        {
            var query = context.ReadContentSet.Where(x => x.FNumber.Equals(read.fnumber)).FirstOrDefault();
            query.FJsonData = JsonHelper.SerializeObject(read);
            query.FModifyTime = DateTime.Now;
            return context.SaveChangesAsync();
        }

        public ReadContent GetReadContent(Read read)
        {
            ReadContent readContent = new ReadContent();
            readContent.FNumber = read.fnumber;
            readContent.FJsonData = JsonHelper.SerializeObject(read);
            readContent.FCreateTime = DateTime.Now;
            readContent.FModifyTime = DateTime.Now;
            return readContent;
        }

    }
}
