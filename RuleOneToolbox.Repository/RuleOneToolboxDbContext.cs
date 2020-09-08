using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RuleOneToolbox.DTO.Models;

namespace RuleOneToolbox.Repository
{
    public class RuleOneToolboxDbContext : DbContext
    {
        private readonly IConfiguration _ObjConfiguration;
        public RuleOneToolboxDbContext(IConfiguration configuration)
        {
            _ObjConfiguration = configuration;
        }

        public RuleOneToolboxDbContext(DbContextOptions<RuleOneToolboxDbContext> options, IConfiguration configuration) : base(options)
        {
            _ObjConfiguration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_ObjConfiguration.GetConnectionString("RuleOneToolBoxDB"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<BalanceSheet> BalanceSheets { get; set; }
        public virtual DbSet<CompanyDetail> CompanyDetails { get; set; }
        public virtual DbSet<CashFlow> CashFlows { get; set; }
    }
}
