namespace school_management_system.DbModels
{
    public class Discipline
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
