using System.ComponentModel.DataAnnotations;

namespace CallCenterCRM.Models
{
    public class BaseModel
    {
        [Display(Name = "Дата создания")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTimeOffset? CreatedDate { get; set; }
        [Display(Name = "Дата обновления")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
