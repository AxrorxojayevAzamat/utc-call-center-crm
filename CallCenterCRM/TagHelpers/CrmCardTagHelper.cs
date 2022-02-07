using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CallCenterCRM.TagHelpers
{
    public class CrmCardTagHelper : TagHelper
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "card");
            output.Content.SetHtmlContent(
                $"<div class=\"card-body\">" +
                $"<h4 class=\"card-title\"> {Title}</h4>" +
                $"<p class=\"card-description\"> {Description}</p>" +
                $"{context}</div>");
        }
    }
}
//< div class= "card" >

//< div class= "card-body" >


//< h4 class= "card-title" > Basic Table </ h4 >


//    < p class= "card-description" >
//        Add class <code>.table </ code >


//        </ p >



//    </ div >

//    </ div >