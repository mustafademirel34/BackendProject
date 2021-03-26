using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        //burada henüz validation yapılmamış
        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)//+password
        {
            byte[] passwordHash, passwordSalt; 
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);//kullanıcı şifresi tuzlanır(yorumlanır) ve şifrelenir

            //UserForRegisterDtokarşıdan aldığımız veri, burada ise kullanıcı nesnesi oluşturuluyor

            var user = new User 
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash, //
                PasswordSalt = passwordSalt, //
                Status = true
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, "Kayıt edildi");
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);//maili ile kullanıcıya ulaşılır
            if (userToCheck == null)//yoksa hata
            {
                return new ErrorDataResult<User>("Kullanıcı bulunamadı");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt)) // login edilen şifre ve veritabanındaki bilgileri kıyaslar
            {
                return new ErrorDataResult<User>("Şifre hatası");
            }

            return new SuccessDataResult<User>(userToCheck, "Giriş başarılı");
        }

        public IResult UserExists(string email) // kullanıcı varlığı kontrolü
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult("Kullanıcı zaten var");
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)//token oluşturma
        {

            var claims = _userService.GetClaims(user);// sadece roller döner

            //kullanıcı bilgisi ve rolleri burada veriliyor //*
            var accessToken = _tokenHelper.CreateToken(user, claims);//token oluşturulur
            return new SuccessDataResult<AccessToken>(accessToken, "Token üretildi");
        }
    }
}
