﻿// <auto-generated />
using System;
using CallCenterCRM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CallCenterCRM.Migrations
{
    [DbContext(typeof(CallcentercrmContext))]
    [Migration("20220403134623_Change-coloumns-app")]
    partial class Changecoloumnsapp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_unicode_ci")
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("CallCenterCRM.Models.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int(11)");

                    b.Property<int?>("AttachmentId")
                        .HasColumnType("int(11)");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int(11)");

                    b.Property<string>("Conclusion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Executor")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("RegisterNumber")
                        .HasColumnType("int(11)");

                    b.Property<string>("ResponseLetter")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ResponsiblePerson")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "AuthorId" }, "Answers_fk1");

                    b.HasIndex(new[] { "ApplicationId" }, "ApplicationId")
                        .IsUnique();

                    b.HasIndex(new[] { "AttachmentId" }, "AttachmentId")
                        .IsUnique();

                    b.ToTable("answers");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Applicant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("BirthDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2022, 4, 3, 18, 46, 23, 410, DateTimeKind.Local).AddTicks(9516));

                    b.Property<int?>("CityDistrictId")
                        .HasColumnType("int(11)");

                    b.Property<bool>("Confidentiality")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Employment")
                        .HasColumnType("int");

                    b.Property<string>("ExtraContact")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Maxalla")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Middlename")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int(11)");

                    b.Property<int>("ReferenceSource")
                        .HasColumnType("int");

                    b.Property<int>("Region")
                        .HasColumnType("int(11)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "CityDistrictId" }, "Applicants_fk0");

                    b.HasIndex(new[] { "OrganizationId" }, "Applicants_fk1");

                    b.ToTable("applicants");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("AdditionalNote")
                        .HasColumnType("text");

                    b.Property<int>("ApplicantId")
                        .HasColumnType("int(11)");

                    b.Property<int?>("AttachmentId")
                        .HasColumnType("int(11)");

                    b.Property<int>("ClassificationId")
                        .HasColumnType("int(11)");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("ExpireTime")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDelayed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MeaningOfApplication")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<int>("RecipientId")
                        .HasColumnType("int(11)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ClassificationId" }, "Application_fk0");

                    b.HasIndex(new[] { "RecipientId" }, "Application_fk1");

                    b.HasIndex(new[] { "ApplicantId" }, "Application_fk3");

                    b.HasIndex(new[] { "AttachmentId" }, "AttachmentId")
                        .IsUnique()
                        .HasDatabaseName("AttachmentId1");

                    b.ToTable("application");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("HashName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("OriginName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("attachments");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Citydistrict", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Region")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("citydistrict");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Classification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("classification");
                });

            modelBuilder.Entity("CallCenterCRM.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("ClassificationId")
                        .HasColumnType("int(11)");

                    b.Property<string>("Contact")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Firstname")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("IdentityId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Middlename")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("ModeratorId")
                        .HasColumnType("int(11)");

                    b.Property<string>("PassportData")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Email" }, "Email")
                        .IsUnique();

                    b.HasIndex(new[] { "Username" }, "Username")
                        .IsUnique();

                    b.HasIndex(new[] { "ModeratorId" }, "Users_fk0");

                    b.HasIndex(new[] { "ClassificationId" }, "Users_fk1");

                    b.ToTable("users");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Answer", b =>
                {
                    b.HasOne("CallCenterCRM.Models.Application", "Application")
                        .WithOne("Answer")
                        .HasForeignKey("CallCenterCRM.Models.Answer", "ApplicationId")
                        .IsRequired()
                        .HasConstraintName("Answers_fk2");

                    b.HasOne("CallCenterCRM.Models.Attachment", "Attachment")
                        .WithOne("Answer")
                        .HasForeignKey("CallCenterCRM.Models.Answer", "AttachmentId")
                        .HasConstraintName("Answers_fk0");

                    b.HasOne("CallCenterCRM.Models.User", "Author")
                        .WithMany("Answers")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("Attachment");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Applicant", b =>
                {
                    b.HasOne("CallCenterCRM.Models.Citydistrict", "CityDistrict")
                        .WithMany("Applicants")
                        .HasForeignKey("CityDistrictId")
                        .HasConstraintName("Applicants_fk0");

                    b.HasOne("CallCenterCRM.Models.User", "Organization")
                        .WithMany("Applicants")
                        .HasForeignKey("OrganizationId")
                        .HasConstraintName("Applicants_fk1");

                    b.Navigation("CityDistrict");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Application", b =>
                {
                    b.HasOne("CallCenterCRM.Models.Applicant", "Applicant")
                        .WithMany("Applications")
                        .HasForeignKey("ApplicantId")
                        .IsRequired()
                        .HasConstraintName("Application_fk3");

                    b.HasOne("CallCenterCRM.Models.Attachment", "Attachment")
                        .WithOne("Application")
                        .HasForeignKey("CallCenterCRM.Models.Application", "AttachmentId")
                        .HasConstraintName("Application_fk2");

                    b.HasOne("CallCenterCRM.Models.Classification", "Classification")
                        .WithMany("Applications")
                        .HasForeignKey("ClassificationId")
                        .IsRequired()
                        .HasConstraintName("Application_fk0");

                    b.HasOne("CallCenterCRM.Models.User", "Recipient")
                        .WithMany("Applications")
                        .HasForeignKey("RecipientId")
                        .IsRequired()
                        .HasConstraintName("Application_fk1");

                    b.Navigation("Applicant");

                    b.Navigation("Attachment");

                    b.Navigation("Classification");

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("CallCenterCRM.Models.User", b =>
                {
                    b.HasOne("CallCenterCRM.Models.Classification", "Classification")
                        .WithMany("Users")
                        .HasForeignKey("ClassificationId")
                        .HasConstraintName("Users_fk1");

                    b.HasOne("CallCenterCRM.Models.User", "Moderator")
                        .WithMany("Organizations")
                        .HasForeignKey("ModeratorId")
                        .HasConstraintName("Users_fk0");

                    b.Navigation("Classification");

                    b.Navigation("Moderator");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Applicant", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Application", b =>
                {
                    b.Navigation("Answer");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Attachment", b =>
                {
                    b.Navigation("Answer");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Citydistrict", b =>
                {
                    b.Navigation("Applicants");
                });

            modelBuilder.Entity("CallCenterCRM.Models.Classification", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("CallCenterCRM.Models.User", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Applicants");

                    b.Navigation("Applications");

                    b.Navigation("Organizations");
                });
#pragma warning restore 612, 618
        }
    }
}
