using System.Text;

namespace WebApp.Template.UserCards
{
    public class PrimeUserCardTemplate : UserCardTemplate
    {
        protected override string SetFooter()
        {
            var sb = new StringBuilder();
            sb.Append("<a href='#' class='card-link'>Mesaj Gönder</a>");
            sb.Append("<a href='#' class='card-link'>Profili Görüntüle</a>");
            return sb.ToString();
        }

        protected override string SetPicture()
        {

            return $"<img src='{appUser.PictureUrl}' class='card-img-top'>";
        }
    }
}
