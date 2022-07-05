using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Context
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Fatura> Faturas { get; set; }
        public DbSet<Ziyaret> Ziyarets { get; set; }
        public DbSet<Sube> Subes { get; set; }
        public DbSet<durumogeleri> durumogeleris { get; set; }
        public DbSet<ZiyaretGecmis> ZiyaretGecmis { get; set; }
        public DbSet<Doviz> Dovizs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<durumogeleri>()
                    .HasMany(e => e.ZiyaretGecmis)
                    .WithRequired(e => e.Durumogeleri)
                    .HasForeignKey(e => e.DurumId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                    .HasMany(e => e.ZiyaretGecmis)
                    .WithRequired(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sube>()
                    .HasMany(e => e.ZiyaretGecmis)
                    .WithRequired(e => e.Sube)
                    .HasForeignKey(e => e.SubeId)
                    .WillCascadeOnDelete(false);



            modelBuilder.Entity<Sube>()
                    .HasMany(e => e.Ziyaret)
                    .WithRequired(e => e.Sube)
                    .HasForeignKey(e => e.SubeId)
                    .WillCascadeOnDelete(false);
        }
    }
}
