﻿using CallCenterCRM.Data;
using CallCenterCRM.Features.Identity;
using CallCenterCRM.Interfaces;
using CallCenterCRM.Models;
using Microsoft.AspNetCore.Identity;

namespace CallCenterCRM.Services
{
    public class UserService : IUserService
    {
        private readonly CallcentercrmContext _context;

        public UserService(CallcentercrmContext context)
        {
            _context = context;
        }

        public int GetUserId(string userIdentityId)
        {
            int userId = getUserByIdentityId(userIdentityId).Id;

            return userId;
        }

        public string GetUserTitle(string userIdentityId)
        {
            string userTitle = getUserByIdentityId(userIdentityId).Title;

            return userTitle;
        }

        public string GetUserRole(string userIdentityId)
        {
            Roles userRole = getUserByIdentityId(userIdentityId).Role;

            return userRole.GetDisplayName();
        }

        private User getUserByIdentityId(string userIdentityId)
        {
            Guid valueIdentityId = Guid.Empty;
            valueIdentityId = new Guid(userIdentityId);
            User user = _context.Users.FirstOrDefault(d => d.IdentityId == valueIdentityId);
            
            return user;
        }
    }
}