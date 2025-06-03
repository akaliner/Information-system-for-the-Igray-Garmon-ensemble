using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _1.Models
{
    public class ScheduleViewModel
    {
        [Required]
        public int Id { get; set; }
        public int EventId { get; set; }
        public string? EventName { get; set; } // если нужно отобразить имя события

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public string? AdditionalInfo { get; set; }

        [Required(ErrorMessage = "Выберите хотя бы одну роль")]
        public List<int> RoleIds { get; set; } = new List<int>();
    }
}
