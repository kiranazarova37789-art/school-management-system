namespace school_management_system.DbModels
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public int UserIdFk { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
