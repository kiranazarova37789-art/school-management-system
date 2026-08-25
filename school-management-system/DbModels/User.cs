using school_management_system.DbModels.Enums;

namespace school_management_system.DbModels
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; }

        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
