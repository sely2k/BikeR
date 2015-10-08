namespace BikeR.Web.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using BikeR.Web.Models;

    public partial class BikeRContext : DbContext
    {
        public BikeRContext()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<NfcField> NfcField { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NfcField>()
                .Property(e => e.C__createdAt)
                .HasPrecision(3);

            modelBuilder.Entity<NfcField>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<NfcField>()
                .Property(e => e.C__version)
                .IsFixedLength();
        }
    }
}
