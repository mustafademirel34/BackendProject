using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Hashing
{
    // araçlar çıplak kalabilir gerke yok
    public class HashingHelper
    {
        public static void CreatePasswordHash // şifre oluşturken
            (string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) // gmacsha512 şifreleme türü
            {
                passwordSalt = hmac.Key; // bir şifreleme eklentisi oluşturulur ve 
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // kullanıcının şifresi bu eklentiyle hesaplanır

                //Salt ve Hash'i veritabanında saklarız, bunu kullanırız

                //Salt kullanıcının şifresine olan kendi yorumumuz,
                //Hash kullanıcının hesaplanmış şifresidir

                // out ile parametre şeklinde referans verilen bu bilgileri burada değiştirdiğimiz zaman
                // gönderilen yerde de değişmektedir. yani kullanıcı bilgileri bu bilgilerle dolmuştur.
            }
        }

        // şifreyi vererek veritabanındaki passwordHash'i alamaz veya kıyaslayamazsın. Bunun için salt'a da ihtiyaç var.
        // Veritabanındaki kullanıcı şifresini çözmek için kullanılan anahtar o'dur.

        public static bool VerifyPasswordHash // şifreyi doğrularken
            (string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))//şifreleme algoritmasını salt'ı veririz (bunu kullandık tarzında)
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));//kullanıcının şifresini bu salt ile tekrar hesaplanır
                for (int i = 0; i < computedHash.Length; i++)//kullanıcının giriş yapmaya çalışırken yeni girdiği şifresi, veritabanındaki şifresiyle kıyaslanır
                {
                    if (computedHash[i] != passwordHash[i])//tek tek, sıra sıra
                    {
                        return false;// bir harfin yanlış çıkması sonucunda hata döner
                    }
                    //kontrol byte şeklinde yapılır, 01001011
                }
                return true;
            }
        }
    }
}
