using CallCenterCRM.Models;

namespace CallCenterCRM.Interfaces
{
    public interface IUserService
    {
        public int GetUserId(string userIdentityId);

        public string GetUserTitle(string userIdentityId);

        public string GetUserRole(string userIdentityId);
        public Roles GetRole(string userIdentityId);

        public bool HasBranches(string userIdentityId);
        public bool UserExists(string userIdentityId);

    }
}
