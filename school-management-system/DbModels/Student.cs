namespace school_management_system.DbModels
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int UserIdFk { get; set; }
        public int ClassIdFk { get; set; }

        public User User { get; set; } = null!;
        public Class Class { get; set; } = null!;
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
