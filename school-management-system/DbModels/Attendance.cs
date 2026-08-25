using school_management_system.DbModels.Enums;

namespace school_management_system.DbModels
{
    public class Attendance
    {
        public int Id { get; set; }
        public int StudentIdFk { get; set; }
        public int ScheduleIdFk { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatus Status { get; set; }

        public Student Student { get; set; } = null!;
        public Schedule Schedule { get; set; } = null!;
    }
}
