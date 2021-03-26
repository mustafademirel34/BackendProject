using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        // Business'daki managerların iş kurallarını çalıştıracağımız sınıf
        // 
        public static IResult Run(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Success) // logic'in çalıştırıldığı yer
                {
                    return logic;//herhangi bir başarısızlıkta metotu döndürür, IResult-ErrorResult
                }
            }
            return null;
        }
    }
}
