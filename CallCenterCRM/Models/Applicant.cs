﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CallCenterCRM.Models
{
    [Table("applicants")]
    [Index(nameof(CityDistrictId), Name = "Applicants_fk0")]
    [Index(nameof(OrganizationId), Name = "Applicants_fk1")]
    public partial class Applicant
    {
        public Applicant()
        {
            Applications = new HashSet<Application>();
        }

        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(255)]
        public string ReferenceSource { get; set; } = null!;
        [StringLength(255)]
        public string Surname { get; set; } = null!;
        [StringLength(255)]
        public string Firstname { get; set; } = null!;
        [StringLength(255)]
        public string Middlename { get; set; } = null!;
        [StringLength(255)]
        public string Contact { get; set; } = null!;
        [StringLength(255)]
        public string ExtraContact { get; set; } = null!;
        [StringLength(255)]
        public string Region { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int CityDistrictId { get; set; }
        [StringLength(255)]
        public string Maxalla { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Address { get; set; } = null!;
        [StringLength(255)]
        public string Sex { get; set; } = null!;
        [Column(TypeName = "timestamp")]
        public DateTime BirthDate { get; set; }
        [StringLength(255)]
        public string Type { get; set; } = null!;
        [StringLength(255)]
        public string Employment { get; set; } = null!;
        [Column(TypeName = "int(255)")]
        public int NumberOfApplication { get; set; }
        public bool Confidentiality { get; set; }
        [Column(TypeName = "text")]
        public string MeaningOfApplication { get; set; } = null!;
        [Column(TypeName = "text")]
        public string AdditionalNote { get; set; } = null!;
        [Column(TypeName = "int(255)")]
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(CityDistrictId))]
        [InverseProperty(nameof(Citydistrict.Applicants))]
        public virtual Citydistrict CityDistrict { get; set; } = null!;
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(User.Applicants))]
        public virtual User Organization { get; set; } = null!;
        [InverseProperty(nameof(Application.Applicant))]
        public virtual ICollection<Application> Applications { get; set; }
    }
}