using UserService.DAL.Abstractions;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Entities
{
    public class UserEntity : BaseEntity, IAuditable
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActivated { get; set; } = false;
        public DateTime CreatedAt { get; set; }

        public Guid RoleId { get; set; }
        public RoleEntity? Role { get; set; }
    }
}
