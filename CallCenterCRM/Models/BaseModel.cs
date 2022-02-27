using System.ComponentModel.DataAnnotations;

namespace CallCenterCRM.Models
{
    public class BaseModel
    {
        [Display(Name = "Дата создания")]
        public DateTimeOffset? CreatedDate { get; set; }
        [Display(Name = "Дата обновления")]
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
