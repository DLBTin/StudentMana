using Microsoft.EntityFrameworkCore;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class AuthService
    {
        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            using var context = new StudentManagementContext();
            return await context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);
        }
    }
}
