using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    // iresult içerisinde gerekli olan imzalar zaten olduğundan tekrar etmek yerine
    // implement ediyoruz, ayrıyeten dataresult bir data döndüreceği için tip<T> belirtiyoruz
    public interface IDataResult<T>:IResult
    {
        T Data { get; }
    }
}
