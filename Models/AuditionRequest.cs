using System.ComponentModel.DataAnnotations;

namespace _1.Models
{
    public class AuditionRequest
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        [Phone(ErrorMessage = "Введите корректный номер телефона")]
        public string PhoneNumber { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public virtual Audition Audition { get; set; }
    }
}