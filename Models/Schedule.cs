using NuGet.DependencyResolver;

namespace _1.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Location { get; set; }
        public string? AdditionalInfo { get; set; }
        public virtual ICollection<Audition> Auditions { get; set; }
        public virtual ICollection<ScheduleRole> ScheduleRoles { get; set; }

    }
}
