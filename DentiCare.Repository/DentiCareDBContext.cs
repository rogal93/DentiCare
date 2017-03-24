using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using DentiCare.Repository.Entities;

namespace DentiCare.Repository
{
    public class DentiCareDBContext : IdentityDbContext
    {
        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<MedicalExperience> MedicalExperience { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<TreatmentCategory> TreatmentCategories { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalDocument> Documents { get; set; }

        public DentiCareDBContext() : base("DefaultConnection") { }

        public static DentiCareDBContext Create()
        {
            return new DentiCareDBContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}