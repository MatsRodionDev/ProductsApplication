using UserService.DAL.Abstractions;

namespace UserService.DAL.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActivated { get; set; } = false;
        public Guid RoleId { get; set; }

        public RoleEntity? Role { get; set; }
    }
}
