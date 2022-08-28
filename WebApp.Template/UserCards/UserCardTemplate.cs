using BaseProject.Models;
using System;
using System.Text;

namespace WebApp.Template.UserCards
{
    public abstract class UserCardTemplate
    {
        protected AppUser appUser { get; set; }
        public void SetUser(AppUser appUser)
        {
            this.appUser = appUser;
        }
        public string Build()
        {
            if (appUser == null) throw new ArgumentNullException(nameof(this.appUser));
            var sb = new StringBuilder();
            sb.Append("<div class='card'>");
            sb.Append(SetPicture());
            sb.Append($@"<div class='card-body'><h5>{this.appUser.UserName}</h5><p>{this.appUser.Description}</p>");
            sb.Append(SetFooter());
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }
        protected abstract string SetFooter();
        protected abstract string SetPicture();
    }
}
