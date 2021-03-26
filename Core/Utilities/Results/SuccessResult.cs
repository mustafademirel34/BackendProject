using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    // test yap class'ı abstract yap
    public class SuccessResult:Result
    {
       //Result implemantasyonu alır, kullanıcı mesaj vermişse veya vermemişse buradan base'gönderir.
        public SuccessResult(string message) : base(true, message) 
        {
           
        }

        public SuccessResult():base(true)
        {

        }
    }
}
