namespace school_management_system.DbModels
{
    public class Schedule
    {
        public int Id { get; set; }
        public int ClassIdFk { get; set; }
        public int TeacherIdFk { get; set; }
        public int DisciplineIdFk { get; set; }
        public short DayOfWeek { get; set; }
        public short LessonNumber { get; set; }
        public string RoomNumber { get; set; } = null!;

        public Class Class { get; set; } = null!;
        public Teacher Teacher { get; set; } = null!;
        public Discipline Discipline { get; set; } = null!;
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
