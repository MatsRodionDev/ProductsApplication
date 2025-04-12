using Shared.Enums;
using System.Text.Json.Serialization;
using UserService.DAL.Abstractions;

namespace UserService.DAL.Entities
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public List<UserEntity> Users { get; set; } = [];
    }
}
