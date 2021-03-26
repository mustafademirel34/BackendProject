using Core.Entities.Concrete;
using Core.Utilities.Security.Encryption;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Core.Extensions; //*
using System.Linq;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        // microsoft.extensions.configuration - apideki appsettins.json dosyasını okumamızı sağlar
        public IConfiguration Configuration { get; }

        // okunan json dosyasındaki verileri geçireceğimiz sınıf, TokenOptions biz oluşturduk
        private TokenOptions _tokenOptions;

        // oluşturulan token ne zaman geçersiz yapılacağının tarihi
        private DateTime _accessTokenExpiration;

        public JwtHelper(IConfiguration configuration) // dahil edildiğinde dosyayı okumaya olanak sağlar
        {
            Configuration = configuration;
            //json dosyasını okuyup bilgileri sınıfımıza aktaran yapı
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

        }
        // kullanıcı token oluşturma
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            // şuanın zamanına json dosyada belirlediğimiz kadar dakika ekler
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            // verdiğimiz güvenlik anahtarını üzerinden gerçeği oluşturulur
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            //ayarlardaki verdiğimiz ga ile giriş bilgisi hesaplanır
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);

            // bu bilgiler ile token bilgileri hesaplanır,
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);


            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);//token oluşturulur

            return new AccessToken // sınıfımızın şekliyle dönüş sağlar+
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        // önemli durumlar için ayrı metotlar oluşturulmuş.
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            //JwtSecurityToken token oluşturmaya yarar, claim bilgileri ister
            //elimizdekileri teslim ediyoruz
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),//kullanıcı için bilgiler ayarlanır
                signingCredentials: signingCredentials // giriş bilgisi
            );
            return jwt;
        }

        //burada kullanıcıya verilen rolleri tanımlanır
        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims) //operationClaims bütün metotları gezdi ^^
        {
            var claims = new List<Claim>();

            //claims.Add(new Claim(JwtRegisteredClaimNames.Email,email)); -- şeklinde yapabilirdik

            //ancak burada metot genişletmeye gidilmiş, Core.Extensions.ClaimExtensions
            
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());//operationClaims, rolleri. 

            return claims;
        }
    }
}
