﻿using CallCenterCRM.Models;
using CallCenterCRM.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallCenterCRM.Forms
{
    public class ApplicantAppInput
    {
        public ApplicantAppInput()
        {
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

            AppTypesList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>();

            AppTypesList = ListEnums.GetEnumList<AppTypes>(AppTypesList);
            StatusList = ListEnums.GetEnumList<ApplicationStatus>(StatusList);
        }

        [Key]
        [Column(TypeName = "int(11)")]
        public int AppId { get; set; }
        public string? AppNum { get; set; } = null!;

        [Display(Name = "Источник обращения")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public ReferenceSources ReferenceSource { get; set; }

        [StringLength(255)]
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public string Surname { get; set; } = null!;

        [StringLength(255)]
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public string Firstname { get; set; } = null!;

        [StringLength(255)]
        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public string Middlename { get; set; } = null!;

        [StringLength(255)]
        [Display(Name = "Контакт")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public string Contact { get; set; } = null!;

        [StringLength(255)]
        [Display(Name = "Дополнительный контакт")]
        public string? ExtraContact { get; set; } = null!;

        [Column(TypeName = "int(11)")]
        [Display(Name = "Регион")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public Regions Region { get; set; }

        [Display(Name = "Город или район")]
        public int? CityDistrictId { get; set; }

        [StringLength(255)]
        [Display(Name = "Махалла")]
        public string? Maxalla { get; set; } = null!;

        [Column(TypeName = "text")]
        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public string Address { get; set; } = null!;

        [Display(Name = "Пол")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public Genders Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Тип заявителя")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public Types Type { get; set; }

        [Display(Name = "Занятость")]
        public Employments? Employment { get; set; }

        [Display(Name = "Конфиденциальность обращение")]
        public bool Confidentiality { get; set; }

        [Column(TypeName = "int(11)")]
        [Display(Name = "Классификация")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public int ClassificationId { get; set; }

        [Column(TypeName = "datetime")]
        [Display(Name = "Срок")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? ExpireTime { get; set; }

        [Display(Name = "Тип обращения")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public AppTypes AppType { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Комментарий")]
        public string? Comment { get; set; } = null!;

        [Column(TypeName = "text")]
        [Display(Name = "Причина")]
        public string? Reason { get; set; } = null!;

        [Column(TypeName = "int(11)")]
        [Display(Name = "Получатель")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public int RecipientId { get; set; }

        [Column(TypeName = "int(11)")]
        [Display(Name = "Вложение ")]
        public int? AttachmentId { get; set; }

        [Display(Name = "Заявител")]
        [Column(TypeName = "int(11)")]
        public int ApplicantId { get; set; }

        [Display(Name = "Суть обращения")]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        public string MeaningOfApplication { get; set; } = null!;

        [Display(Name = "Дополнительное примечание к заявлении")]
        public string? AdditionalNote { get; set; }

        public string? AuthorName { get; set; }

        [StringLength(255)]
        [Display(Name = "Наименование организации")]
        public string? OrganizationName { get; set; }

        [StringLength(255)]
        [Display(Name = "СТИР")]
        public string? Stir { get; set; }

        [NotMapped]
        public int? OrganizationId { get; set; }

        public ApplicationStatus AppStatus { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? AppCreatedDate { get; set; }


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
        [NotMapped]
        public List<SelectListItem> AppTypesList { get; set; }
        [NotMapped]
        public List<SelectListItem> StatusList { get; set; }
        /* / notmapped*/
    }
}
