namespace _1.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
