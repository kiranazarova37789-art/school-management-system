using Microsoft.EntityFrameworkCore;
using school_management_system.Services.Interfaces;
using school_management_system.ViewModels;
using SchoolProject.Data;

namespace school_management_system.Services
{
    public class ProfileService : IProfileService
    {
        private readonly SchoolDbContext _context;
        public ProfileService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<StudentProfileViewModel> GetStudentProfileAsync(int userId)
        {
            var student = await _context.Students.Include(s => s.Class).FirstOrDefaultAsync(u => u.UserIdFk == userId);

            if (student == null)
            {
                return null;
            }

            return new StudentProfileViewModel
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                MiddleName = student.MiddleName,
                DateOfBirth = student.DateOfBirth,
                ClassName = student.Class.Name
            };
        }
    }
}
