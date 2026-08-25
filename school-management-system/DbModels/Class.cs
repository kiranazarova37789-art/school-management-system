namespace school_management_system.DbModels
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string AcademicYear { get; set; } = null!;

        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
