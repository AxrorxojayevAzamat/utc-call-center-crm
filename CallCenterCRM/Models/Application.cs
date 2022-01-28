﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CallCenterCRM.Models
{
    [Table("application")]
    [Index(nameof(ClassificationId), Name = "Application_fk0")]
    [Index(nameof(UserId), Name = "Application_fk1")]
    [Index(nameof(ApplicantId), Name = "Application_fk3")]
    [Index(nameof(AttachmentId), Name = "AttachmentId", IsUnique = true)]
    public partial class Application
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }
        [Column(TypeName = "int(11)")]
        public int Direction { get; set; }
        [StringLength(255)]
        public string Value { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int ClassificationId { get; set; }
        [StringLength(255)]
        public string Recipient { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime ExpireTime { get; set; }
        [StringLength(255)]
        public string RelevantApplications { get; set; } = null!;
        [StringLength(255)]
        public string Type { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Comment { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int UserId { get; set; }
        [Column(TypeName = "int(11)")]
        public int AttachmentId { get; set; }
        [Column(TypeName = "int(11)")]
        public int ApplicantId { get; set; }

        [ForeignKey(nameof(ApplicantId))]
        [InverseProperty("Applications")]
        public virtual Applicant Applicant { get; set; } = null!;
        [ForeignKey(nameof(AttachmentId))]
        [InverseProperty("Application")]
        public virtual Attachment Attachment { get; set; } = null!;
        [ForeignKey(nameof(ClassificationId))]
        [InverseProperty("Applications")]
        public virtual Classification Classification { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Applications")]
        public virtual User User { get; set; } = null!;
        [InverseProperty("Application")]
        public virtual Answer Answer { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}