using System.ComponentModel.DataAnnotations;

namespace CallCenterCRM.Forms
{
    public class PasswordChangeInput
    {
        public int UserId { get; set; }
        [Display(Name = "Старый пароль")]
        public string Password { get; set; } = null!;
        [Compare("Password", ErrorMessage = "Пароль неверно!")]
        public string OldPassword { get; set; } = null!;

        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; } = null!;
        [Display(Name = "Подтверждать новый пароль")]
        [Compare("NewPassword", ErrorMessage = "Пароль и пароль подтверждения не совпадают!")]
        public string ConfirmPassword { get; set; } = null!;
        public DateTimeOffset? CreatedDate { get; set; }
    }
}
