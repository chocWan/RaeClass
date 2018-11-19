using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Models
{
    public class RaeClassContext:DbContext
    {

        public RaeClassContext(DbContextOptions<RaeClassContext> options)
            : base(options)
        {
        }

        public DbSet<ReadContent> ReadContentSet { get; set; }
        public DbSet<ListenContent> ListenContentSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //自定义表名
            modelBuilder.Entity<ReadContent>().ToTable("ReadContent", "dbo");
            modelBuilder.Entity<ListenContent>().ToTable("ListenContent", "dbo");

            base.OnModelCreating(modelBuilder);
        }

    }
}
