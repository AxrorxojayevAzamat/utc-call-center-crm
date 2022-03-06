namespace CallCenterCRM.Interfaces
{
    public interface IUserService
    {
        public int GetUserId(string userIdentityId);

        public string GetUserTitle(string userIdentityId);

        public string GetUserRole(string userIdentityId);
    }
}
