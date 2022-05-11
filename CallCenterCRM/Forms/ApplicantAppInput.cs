using CallCenterCRM.Models;
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
        public string Middlename { get; set; } = null!;

        [StringLength(255)]
        [Display(Name = "Контакт")]
        public string Contact { get; set; } = null!;

        [StringLength(255)]
        [Display(Name = "Дополнительный контакт")]
        public string? ExtraContact { get; set; } = null!;

        [Column(TypeName = "int(11)")]
        [Display(Name = "Регион")]
        public Regions Region { get; set; }

        [Display(Name = "Город или район")]
        public int? CityDistrictId { get; set; }

        [StringLength(255)]
        [Display(Name = "Махалла")]
        public string? Maxalla { get; set; } = null!;

        [Column(TypeName = "text")]
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

        [Column(TypeName = "int(11)")]
        [Display(Name = "Классификация")]
        public int ClassificationId { get; set; }

        [Column(TypeName = "datetime")]
        [Display(Name = "Срок")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:00}")]
        public DateTime ExpireTime { get; set; }

        [Display(Name = "Тип обращения")]
        public AppTypes AppType { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Комментарий")]
        public string? Comment { get; set; } = null!;

        [Column(TypeName = "text")]
        [Display(Name = "Причина")]
        public string? Reason { get; set; } = null!;

        [Column(TypeName = "int(11)")]
        [Display(Name = "Получатель")]
        public int RecipientId { get; set; }

        [Column(TypeName = "int(11)")]
        [Display(Name = "Вложение ")]
        public int? AttachmentId { get; set; }

        [Display(Name = "Заявител")]
        [Column(TypeName = "int(11)")]
        public int ApplicantId { get; set; }

        [Display(Name = "Суть обращения")]
        public string MeaningOfApplication { get; set; } = null!;

        [Display(Name = "Дополнительное примечание к заявлении")]
        public string? AdditionalNote { get; set; }

        [NotMapped]
        public int? OrganizationId { get; set; }

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
