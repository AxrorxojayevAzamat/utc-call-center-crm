using Microsoft.AspNetCore.Mvc.Rendering;

namespace CallCenterCRM.Utilities
{
    public class ListEnums
    {
        public static List<SelectListItem> GetEnumList<T>(List<SelectListItem> selectList) where T : Enum
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                selectList.Add(new SelectListItem
                {
                    Value = (item).ToString(),
                    Text = item.GetDisplayName()
                });
            }
            return selectList;
        }
    }
}
