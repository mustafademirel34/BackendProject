using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
    // var olan sınıfları metotlarla genişletebiliriz
    public static class ClaimExtensions
    {
        // örneğin

        // var claims = new List<Claim>(); - claims.AddEmail(user.Email); şeklinde kullanılırken
        // mail gönderildiğinde claims otomatik alınır, ICollection<Claim> şeklinde, eklenirken burada
        // o koleksiyona bir ekleme oluşturulur

        public static void AddEmail(this ICollection<Claim> claims, string email) // Claim sınıfını AddEmail metotuyla genişlet
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)//roller burada eklenir
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
    }
}
