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
        //[DataType(DataType.Password)]
        [Display(Name = "Ввод пароля")]
        public string Password { get; set; } = null!;

        //[DataType(DataType.Password)]
        [Display(Name = "Подтвердить Пароль")]
        [Compare("Password", ErrorMessage = "Пароль и пароль подтверждения не совпадают.")]
        public string ConfirmPassword { get; set; } = null!;
        [Display(Name = "Роль")]
        public Roles Role { get; set; }

        [StringLength(255)]
        [Display(Name = "Город")]
        public string City { get; set; } = null!;

        [NotMapped]
        public List<SelectListItem> RolesList { get; set; }
    }
}
