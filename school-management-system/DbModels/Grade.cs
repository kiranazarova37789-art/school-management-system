namespace school_management_system.DbModels
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentIdFk { get; set; }
        public int TeacherIdFk { get; set; }
        public int DisciplineIdFk { get; set; }
        public short Value { get; set; }
        public DateTime Date { get; set; }

        public Student Student { get; set; } = null!;
        public Teacher Teacher { get; set; } = null!;
        public Discipline Discipline { get; set; } = null!;
    }
}
