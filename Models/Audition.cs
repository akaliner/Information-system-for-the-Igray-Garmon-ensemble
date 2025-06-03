using System.ComponentModel.DataAnnotations;
namespace _1.Models
{
    public class Audition
    {
        public int Id { get; set; }

        [Required]
        public DateTime AuditionDate { get; set; }
        public int AuditionRequestId { get; set; }
        public AuditionRequest AuditionRequest { get; set; }
        public int ResponsibleEmployeeId { get; set; }
        public User ResponsibleEmployee { get; set; }

    }
}