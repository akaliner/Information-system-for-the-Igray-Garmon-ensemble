namespace _1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        // Дополнительный метод для получения всех ролей пользователя
        public ICollection<Role> Roles => UserRoles?.Select(ur => ur.Role).ToList() ?? new List<Role>();
    }
}
