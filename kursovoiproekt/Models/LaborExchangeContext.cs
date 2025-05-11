using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace kursovoiproekt.Models
{
    public class LaborExchangeContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<JobAd> JobAds { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EmploymentRecord> EmploymentRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=172.20.7.53;Database=db2991_25;Username=st2991;Password=pwd2991;SearchPath=kursovoi");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chats", "kursovoi");
                entity.Property(e => e.JobAdId).HasColumnName("jobadid");
                entity.Property(e => e.EmployerId).HasColumnName("employerid");
                entity.Property(e => e.ApplicantId).HasColumnName("applicantid");
            });
        }

    } }
