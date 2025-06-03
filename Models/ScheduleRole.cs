namespace _1.Models
{
    public class ScheduleRole
    {
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
