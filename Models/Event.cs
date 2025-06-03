namespace _1.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Audition> Auditions { get; set; }
    }
}
