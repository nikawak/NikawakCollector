﻿using CourseProject.Models;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Helpers
{
    public class SeedData : ISeedData
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public SeedData(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task EnsureData()
        {
            await CreateDefaultRoles();
            await CreateDefaultAdmin();
        }

        private async Task<IEnumerable<IdentityRole>> CreateDefaultRoles()
        {
            var roleUser = new IdentityRole()
            {
                Name = "User"
            };
            var roleAdmin = new IdentityRole()
            {
                Name = "Admin"
            };

            await _roleManager.CreateAsync(roleUser);
            await _roleManager.CreateAsync(roleAdmin);


            return new[] { roleUser, roleAdmin };
        }
        private async Task CreateDefaultAdmin()
        {
            var userAdmin = new User()
            {
                UserName = "admin",
                Email = "Nikawak228@gmail.com",
                IsBlocked = false
            };


            var adminResult = await _userManager.CreateAsync(userAdmin, "admin");
            var adminRolesResult = await _userManager.AddToRolesAsync(userAdmin, new[] { "Admin", "User" });

        }
    }
}
