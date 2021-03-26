using Core.DataAccess;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        //operationClaim, yetki,rol anlamında, 
        List<OperationClaim> GetClaims(User user);
    }
}
