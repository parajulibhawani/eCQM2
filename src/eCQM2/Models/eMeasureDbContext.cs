using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Builder;


namespace eCQM2.Models
{
    public class eMeasureDbContext : DbContext
    {
        public DbSet<eMeasure> eMeasures { get; set; }
        public DbSet<Reference> References { get; set; }

        public eMeasureDbContext(DbContextOptions<eMeasureDbContext> options)
            : base(options)
        {
        }

        public eMeasureDbContext() : base()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EmeasureDB;integrated security=True");
        }
    }
}