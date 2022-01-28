﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CallCenterCRM.Models;

namespace CallCenterCRM.Data
{
    public partial class CallcentercrmContext : DbContext
    {
        public CallcentercrmContext()
        {
        }

        public CallcentercrmContext(DbContextOptions<CallcentercrmContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Applicant> Applicants { get; set; } = null!;
        public virtual DbSet<Application> Applications { get; set; } = null!;
        public virtual DbSet<Attachment> Attachments { get; set; } = null!;
        public virtual DbSet<Citydistrict> Citydistricts { get; set; } = null!;
        public virtual DbSet<Classification> Classifications { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;database=callcentercrm;uid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.33-mysql"), x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Application)
                    .WithOne(p => p.Answer)
                    .HasForeignKey<Answer>(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Answers_fk2");

                entity.HasOne(d => d.Attachment)
                    .WithOne(p => p.Answer)
                    .HasForeignKey<Answer>(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Answers_fk0");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Answers_fk1");
            });

            modelBuilder.Entity<Applicant>(entity =>
            {
                entity.HasOne(d => d.CityDistrict)
                    .WithMany(p => p.Applicants)
                    .HasForeignKey(d => d.CityDistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Applicants_fk0");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Applicants)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Applicants_fk1");
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasOne(d => d.Applicant)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.ApplicantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Application_fk3");

                entity.HasOne(d => d.Attachment)
                    .WithOne(p => p.Application)
                    .HasForeignKey<Application>(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Application_fk2");

                entity.HasOne(d => d.Classification)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.ClassificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Application_fk0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Application_fk1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.InverseOrganization)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("Users_fk0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}