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

        public DbSet<SerialNumber> SerialNumberSet { get; set; }
        public DbSet<BaseFormContent> BaseFormContentSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //自定义表名
            modelBuilder.Entity<BaseFormContent>().ToTable("BaseFormContent", "dbo");
            modelBuilder.Entity<SerialNumber>().ToTable("SerialNumber", "dbo");

            base.OnModelCreating(modelBuilder);
        }

    }
}
