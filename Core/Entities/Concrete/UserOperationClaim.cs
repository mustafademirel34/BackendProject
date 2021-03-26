namespace Core.Entities.Concrete
{
    public class UserOperationClaim : IEntity//kullanıcı için rolleri
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
    }
}
