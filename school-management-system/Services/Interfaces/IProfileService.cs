using school_management_system.ViewModels;

namespace school_management_system.Services.Interfaces
{
    public interface IProfileService
    {
        Task<StudentProfileViewModel> GetStudentProfileAsync(int userId);
    }
}
