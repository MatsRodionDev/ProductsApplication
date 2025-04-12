namespace UserService.BLL.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActivated { get; set; } = false;

        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
