using System.Text;

namespace WebApp.Template.UserCards
{
    public class PrimeUserCardTemplate : UserCardTemplate
    {
        protected override string SetFooter()
        {
            var sb = new StringBuilder();

            sb.Append("<a href=\"#\" class=\"btn btn-primary\">Mesaj Gönder</a>");
            sb.Append("<a href=\"#\" class=\"btn btn-primary\">Detaylı Profil</a>");

            return sb.ToString();
        }

        protected override string SetPicture()
        {
            return $"<img src=\"{AppUser.PictureUrl}\" class=\"card-img-top\" alt=\"...\">";
        }
    }
}
