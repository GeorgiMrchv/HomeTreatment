﻿using HomeTreatment.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    }
}