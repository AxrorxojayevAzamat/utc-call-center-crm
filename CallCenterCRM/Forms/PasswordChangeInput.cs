using System.ComponentModel.DataAnnotations;

namespace CallCenterCRM.Forms
{
    public class PasswordChangeInput
    {
        public int UserId { get; set; }
        [Display(Name = "Старый пароль")]
        [DataType(DataType.Password)]
        [Compare("OldPassword", ErrorMessage = "Пароль неверно!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", 
            ErrorMessage = "Парол слабый или менее 8 символов")]
        public string Password { get; set; } = null!;
        public string OldPassword { get; set; } = null!;

        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$",
            ErrorMessage = "Парол слабый или менее 8 символов")]
        public string NewPassword { get; set; } = null!;
        [Display(Name = "Подтверждать новый пароль")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$",
            ErrorMessage = "Парол слабый или менее 8 символов")]
        [Compare("NewPassword", ErrorMessage = "Пароль и пароль подтверждения не совпадают!")]
        public string ConfirmPassword { get; set; } = null!;
        public DateTimeOffset? CreatedDate { get; set; }
    }
}
