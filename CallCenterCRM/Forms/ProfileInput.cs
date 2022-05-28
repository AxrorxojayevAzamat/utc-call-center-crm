using CallCenterCRM.Models;
using CallCenterCRM.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallCenterCRM.Forms
{
    public class ProfileInput
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }

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

        [Column(TypeName = "int(11)")]
        [Display(Name = "Связка с организацией")]
        public int? ModeratorId { get; set; }

        [Display(Name = "Направление")]
        [Column(TypeName = "int(11)")]
        public int? DirectionId { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

    }
}
