using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Web;
using System.Collections.Generic;

namespace CallCenterCRM.ViewComponents
{
    public class CrmTable<T> : ViewComponent
    {
        List<T> _model;
        public CrmTable(List<T> model)
        {
            _model = model;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_model);
        }
    }
}
