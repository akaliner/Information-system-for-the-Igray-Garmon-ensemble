using System.ComponentModel.DataAnnotations;
namespace _1.Models
{
    public class AssignAuditionViewModel
    {
        public int AuditionRequestId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime AuditionDate { get; set; }
    }

}
