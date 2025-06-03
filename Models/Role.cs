﻿namespace _1.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<ScheduleRole> ScheduleRoles { get; set; }
    }
}
