using System;

namespace _1.Models
{
    public class RepertoireEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string HtmlContent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
