using WebApp.Template.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace WebApp.Template.UserCards
{
    //<user-card app-user="value" />
    public class UserCardTagHelper:TagHelper
    {
        public AppUser appUser { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserCardTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            UserCardTemplate template;
            if(_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                template = new PrimeUserCardTemplate();
                
            }
            else
            {
                template = new DefaultUserCardTemplate();
            }
            template.SetUser(appUser);
            output.Content.SetHtmlContent(template.Build());

        }
    }
}
