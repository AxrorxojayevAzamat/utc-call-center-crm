using CallCenterCRM.Models;
using CallCenterCRM.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallCenterCRM.Forms
{
    public class RegisterUserInput
    {
        public RegisterUserInput()
        {

            RolesList = new List<SelectListItem>();

            RolesList = ListEnums.GetEnumList<Roles>(RolesList);
        }

        [StringLength(255)]
        [Display(Name = "Наименование организации")]
        public string Title { get; set; }

        [Display(Name = "Ввод логина")]
        public string Username { get; set; } = null!;

        [Display(Name = "e-mail")]
        public string Email { get; set; }

        [StringLength(255)]
        [Display(Name = "Номер телефона")]
        public string Contact { get; set; } = null!;


        [StringLength(255)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$",
            ErrorMessage = "Парол слабый или менее 8 символов")]
        [Display(Name = "Ввод пароля")]
        public string Password { get; set; } = null!;

        //[DataType(DataType.Password)]
        [Display(Name = "Подтвердить Пароль")]
        [Compare("Password", ErrorMessage = "Пароль и пароль подтверждения не совпадают.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$",
            ErrorMessage = "Парол слабый или менее 8 символов")]
        public string ConfirmPassword { get; set; } = null!;
        [Display(Name = "Роль")]
        public Roles Role { get; set; }

        [StringLength(255)]
        [Display(Name = "Город")]
        public string City { get; set; } = null!;
        
        [Display(Name = "Направление")]
        public int? DirectionId { get; set; }

        [Display(Name = "Модератор")]
        public int? ModeratorId { get; set; }

        [NotMapped]
        public List<SelectListItem> RolesList { get; set; }
    }
}
