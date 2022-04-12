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
    [Table("applicants")]
    [Index(nameof(CityDistrictId), Name = "Applicants_fk0")]
    [Index(nameof(OrganizationId), Name = "Applicants_fk1")]
    public partial class Applicant : BaseModel
    {
        public Applicant()
        {
            Applications = new HashSet<Application>();

            RegionsList = new List<SelectListItem>();
            ReferenceSourcesList = new List<SelectListItem>();
            TypesList = new List<SelectListItem>();
            EmploymentsList = new List<SelectListItem>();
            GendersList = new List<SelectListItem>();

            ReferenceSourcesList = ListEnums.GetEnumList<ReferenceSources>(ReferenceSourcesList);
            RegionsList = ListEnums.GetEnumList<Regions>(RegionsList);
            TypesList = ListEnums.GetEnumList<Types>(TypesList);
            EmploymentsList = ListEnums.GetEnumList<Employments>(EmploymentsList);
            GendersList = ListEnums.GetEnumList<Genders>(GendersList);
        }

        [Key]
        public int Id { get; set; }
        [Display(Name = "Источник обращения")]
        public ReferenceSources ReferenceSource { get; set; }
        [StringLength(255)]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; } = null!;
        [StringLength(255)]
        [Display(Name = "Имя")]
        public string Firstname { get; set; } = null!;
        [StringLength(255)]
        [Display(Name = "Отчество")]
        public string? Middlename { get; set; } = null!;
        [StringLength(255)]
        [Display(Name = "Контакт")]
        public string Contact { get; set; } = null!;
        [StringLength(255)]
        [Display(Name = "Дополнительный контакт")]
        public string? ExtraContact { get; set; } = null!;
        [Display(Name = "Регион")]
        public Regions Region { get; set; }
        [Display(Name = "Город или район")]
        public int? CityDistrictId { get; set; }
        [StringLength(255)]
        [Display(Name = "Махалла")]
        public string? Maxalla { get; set; } = null!;
        [Display(Name = "Адрес")]
        public string Address { get; set; } = null!;
        [Display(Name = "Пол")]
        public Genders Gender { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Тип заявителя")]
        public Types Type { get; set; }
        [Display(Name = "Занятость")]
        public Employments Employment { get; set; }
        [Display(Name = "Конфиденциальность обращение")]
        public bool Confidentiality { get; set; }
        
        [NotMapped]
        //[Display(Name = "Заявление")]
        public int? OrganizationId { get; set; }
        [ForeignKey(nameof(CityDistrictId))]
        [InverseProperty(nameof(Citydistrict.Applicants))]
        public virtual Citydistrict? CityDistrict { get; set; } = null!;
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(User.Applicants))]
        public virtual User? Organization { get; set; } = null!;
        [InverseProperty(nameof(Application.Applicant))]
        public virtual ICollection<Application> Applications { get; set; }

        /*notmapped*/
        [NotMapped]
        public List<SelectListItem> ReferenceSourcesList { get; set; }
        [NotMapped]
        public List<SelectListItem> RegionsList { get; set; }
        [NotMapped]
        public List<SelectListItem> TypesList { get; set; }
        [NotMapped]
        public List<SelectListItem> EmploymentsList { get; set; }
        [NotMapped]
        public List<SelectListItem> GendersList { get; set; }
        /* / notmapped*/

    }
}