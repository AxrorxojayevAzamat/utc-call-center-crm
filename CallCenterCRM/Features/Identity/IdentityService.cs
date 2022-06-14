using CallCenterCRM.Forms;
using CallCenterCRM.Models;
using System.Runtime.Serialization;
using UTC.Auth.Client;

namespace CallCenterCRM.Features.Identity
{

    public class IdentityService
    {

        public IHttpClientFactory HttpClient { get; set; }

        public IdentityService(IHttpClientFactory httpClient)
        {
            this.HttpClient = httpClient;
        }
        public async Task<User> Register(RegisterUserInput _user, string roleName)
        {

            var client = HttpClient.CreateClient("IdentityAPI");
            var usersClient = new UsersClient(client.BaseAddress?.AbsoluteUri, client);
            var rolesClient = new RolesClient(client.BaseAddress?.AbsoluteUri, client);

            var user = await usersClient.CreateAsync(new IdentityUserDto
            {
                UserName = _user.Username,
                Email = _user.Email,
                EmailConfirmed = true,
                PhoneNumber = _user.Contact,
                PhoneNumberConfirmed = _user.Contact is not null,
            });

            await usersClient.ChangePasswordAsync(new UserChangePasswordApiDtoOfString
            {
                UserId = user.Id,
                Password = _user.Password,
                ConfirmPassword = _user.Password,
            });


            var rolesDto = await rolesClient.GetBySearchAsync(roleName, 0, 15);
            var role = rolesDto.Roles[0];

            await usersClient.CreateUserRolesAsync(new UserRoleApiDtoOfString
            {
                UserId = user.Id,
                RoleId = role.Id,
            });

            int roleIndex = (int)Enum.Parse(typeof(Roles), role.Name);

            return new User()
            {
                IdentityId = Guid.Parse(user.Id),
                Role = (Roles)roleIndex
            };
        }

        public async Task<User> UpdateUser(User _user, string roleName, string oldRoleName)
        {

            var client = HttpClient.CreateClient("IdentityAPI");
            var usersClient = new UsersClient(client.BaseAddress?.AbsoluteUri, client);
            var rolesClient = new RolesClient(client.BaseAddress?.AbsoluteUri, client);

            await usersClient.UpdateAsync(new IdentityUserDto
            {
                Id = _user.IdentityId.ToString(),
                UserName = _user.Username,
                Email = _user.Email,
                EmailConfirmed = true,
                PhoneNumber = _user.Contact,
                PhoneNumberConfirmed = _user.Contact is not null,
            });

            int roleIndex = (int)_user.Role;

            if (roleName != oldRoleName)
            {
                var rolesDto = await rolesClient.GetBySearchAsync(roleName, 0, 15);
                var oldRolesDto = await rolesClient.GetBySearchAsync(oldRoleName, 0, 15);

                var role = rolesDto.Roles[0];
                var oldRole = oldRolesDto.Roles[0];


                await usersClient.CreateUserRolesAsync(new UserRoleApiDtoOfString
                {
                    UserId = _user.IdentityId.ToString(),
                    RoleId = role.Id,
                });

                await usersClient.DeleteUserRolesAsync(new UserRoleApiDtoOfString
                {
                    UserId = _user.IdentityId.ToString(),
                    RoleId = oldRole.Id,
                });

                roleIndex = (int)Enum.Parse(typeof(Roles), role.Name);
            }

            return new User()
            {
                Role = (Roles)roleIndex
            };
        }

        public async Task<FileResponse> ChangePassword(PasswordChangeInput passwordChange, Guid identityId)
        {
            var client = HttpClient.CreateClient("IdentityAPI");
            var usersClient = new UsersClient(client.BaseAddress?.AbsoluteUri, client);

            return await usersClient.ChangePasswordAsync(new UserChangePasswordApiDtoOfString
            {
                UserId = identityId.ToString(),
                Password = passwordChange.NewPassword,
                ConfirmPassword = passwordChange.ConfirmPassword,
            });
        }
    }
}
