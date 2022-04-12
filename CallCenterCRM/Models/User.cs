﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CallCenterCRM.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CallCenterCRM.Models
{
    [Table("users")]
    [Index(nameof(Email), Name = "Email", IsUnique = true)]
    [Index(nameof(Username), Name = "Username", IsUnique = true)]
    [Index(nameof(ModeratorId), Name = "Users_fk0")]
    [Index(nameof(ClassificationId), Name = "Users_fk1")]
    public partial class User : BaseModel
    {
        public User()
        {
            Answers = new HashSet<Answer>();
            Applicants = new HashSet<Applicant>();
            Applications = new HashSet<Application>();
            Organizations = new HashSet<User>();

            RolesList = new List<SelectListItem>();

            RolesList = ListEnums.GetEnumList<Roles>(RolesList);
        }

        [Key]
        public int Id { get; set; }

        public Guid IdentityId { get; set; } = Guid.NewGuid();

        [Display(Name = "Ввод логина")]
        public string Username { get; set; } = null!;

        [Display(Name = "e-mail")]
        public string? Email { get; set; }

        [StringLength(255)]
        [Display(Name = "Город")]
        public string City { get; set; } = null!;

        [StringLength(255)]
        [DataType(DataType.Password)]
        [Display(Name = "Ввод пароля")]
        public string Password { get; set; } = null!;


        [Display(Name = "Роль")]
        public Roles Role { get; set; }

        [StringLength(255)]
        [Display(Name = "Номер телефона")]
        public string? Contact { get; set; } = null!; 

        [StringLength(255)]
        [Display(Name = "Наименование организации")]
        public string Title { get; set; }

        [StringLength(255)]
        [Display(Name = "Фамилия")]
        public string? Surname { get; set; }

        [StringLength(255)]
        [Display(Name = "Имя")]
        public string? Firstname { get; set; }

        [StringLength(255)]
        [Display(Name = "Отчество")]
        public string? Middlename { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Паспортные данные")]
        public string? PassportData { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Адрес проживания")]
        public string? Address { get; set; }

        [Column(TypeName = "integer")]
        [Display(Name = "Связка с организацией")]
        public int? ModeratorId { get; set; }

        [ForeignKey(nameof(ModeratorId))]
        [InverseProperty(nameof(User.Organizations))]
        [Display(Name = "Связка с организацией")]
        public virtual User? Moderator { get; set; }

        [Display(Name = "Классификация")]
        [Column(TypeName = "integer")]
        public int? ClassificationId { get; set; }

        [Display(Name = "Классификация")]
        [ForeignKey(nameof(ClassificationId))]
        [InverseProperty(nameof(CallCenterCRM.Models.Classification.Users))]
        public virtual Classification? Classification { get; set; }

        [InverseProperty(nameof(Answer.Author))]
        public virtual ICollection<Answer> Answers { get; set; }

        [InverseProperty(nameof(Applicant.Organization))]
        public virtual ICollection<Applicant> Applicants { get; set; }

        [InverseProperty(nameof(Application.Recipient))]
        public virtual ICollection<Application> Applications { get; set; }

        [InverseProperty(nameof(User.Moderator))]
        public virtual ICollection<User> Organizations { get; set; }

        /*notmapped*/
        [NotMapped]
        public List<SelectListItem> RolesList { get; set; }
        /* / notmapped*/

    }
}