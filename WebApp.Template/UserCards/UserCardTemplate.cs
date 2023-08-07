﻿using System.Text;
using WebApp.Template.Models;

namespace WebApp.Template.UserCards
{
    public abstract class UserCardTemplate
    {
        protected AppUser AppUser { get; set; }

        public void SetUser(AppUser appUser)
        {
            AppUser = appUser;
        }

        public string Build()
        {
            if (AppUser == null) { throw new ArgumentNullException(nameof(AppUser)); }

            var sb = new StringBuilder();

            sb.Append("<div class=\"card\" style=\"width: 18rem;\">");
            sb.Append(SetPicture());
            sb.Append($@"<div class='card-body'>
                            <h5>{AppUser.UserName}</h5>
                            <h5>{AppUser.Description}</h5>");
            sb.Append(SetFooter());
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }
        protected abstract string SetFooter();
        protected abstract string SetPicture();
    }
}
