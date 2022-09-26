using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Clinic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ClinicApi.Models;

namespace Clinic.Data
{
    public partial class ClinicContext : IdentityDbContext<User>
    {
     

        public ClinicContext(DbContextOptions<ClinicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<DoctorSchedule> DoctorSchedules { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("appointments");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AppointmentDate).HasColumnName("appointment_date");

                entity.Property(e => e.BookingDate)
                    .HasColumnName("booking_date")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.DoctorId)
                    .HasColumnType("int(10)")
                    .HasColumnName("doctor_id");

                entity.Property(e => e.PatientId)
                    .HasColumnType("int(10)")
                    .HasColumnName("patient_id");

                entity.Property(e => e.SlotId)
                    .HasColumnType("int(2)")
                    .HasColumnName("slot_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.Property(e => e.TotalSlots)
                    .HasColumnType("int(2)")
                    .HasColumnName("total_slots");
            });

            modelBuilder.Entity<DoctorSchedule>(entity =>
            {
                entity.ToTable("doctor_schedule");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.DoctorId)
                    .HasColumnType("int(10)")
                    .HasColumnName("doctor_id");

                entity.Property(e => e.SlotsAvailable)
                    .HasColumnType("int(2)")
                    .HasColumnName("slots_available");

                entity.Property(e => e.SlotsBooked)
                    .HasColumnType("int(2)")
                    .HasColumnName("slots_booked");

                entity.Property(e => e.TotalAppointments)
                    .HasColumnType("int(2)")
                    .HasColumnName("total_appointments");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.User_Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");


                entity.Property(e => e.RoleId).HasColumnName("role_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
