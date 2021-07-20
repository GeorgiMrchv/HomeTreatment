using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HomeTreatment.Data.Models;

namespace HomeTreatment.Data
{
    public class HomeTreatmentDbContext : IdentityDbContext<User> // po tozi nachin nadgradihme defaultniq Identity
    {
        public HomeTreatmentDbContext(DbContextOptions<HomeTreatmentDbContext> options) : base(options)
        {

        }
        public DbSet<DoctorPatientMessage> DoctorPatientMessages { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.HasOne(d => d.User)
                     .WithOne(u => u.Doctor)
                     .HasForeignKey<Doctor>(d => d.Id);
            });

            builder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasOne(p => p.User)
                     .WithOne(u => u.Patient)
                     .HasForeignKey<Patient>(p => p.Id);
            });

            base.OnModelCreating(builder);
        }
    }
}
