using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using school_management_system.DbModels;
using school_management_system.DbModels.Enums;
using school_management_system.Services.Interfaces;
using SchoolProject.Data;

namespace school_management_system.Services
{
    public class AccountServise : IAccountService
    {
        private readonly SchoolDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountServise (SchoolDbContext context, PasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User?> LoginAcc(string name, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.Name,
                user.PasswordHash);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return user;
        }

        public async Task<bool> RegistrationAcc(string name, string password)
        {
            var userOld = await _context.Users.AnyAsync(u => u.Name == name);

            if (userOld)
                return false;

            var user = new User
            {
                Name = name,
                Role = UserRole.Student};

            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
