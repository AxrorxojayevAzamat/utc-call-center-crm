using System.ComponentModel.DataAnnotations;

namespace CallCenterCRM.Models
{
    public class BaseModel
    {
        [Display(Name = "Дата создания")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm}")]
        public DateTimeOffset? CreatedDate { get; set; }
        [Display(Name = "Дата обновления")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm}")]
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
