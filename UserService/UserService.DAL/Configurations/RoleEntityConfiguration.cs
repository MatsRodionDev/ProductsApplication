using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Enums;
using UserService.DAL.Entities;

namespace UserService.DAL.Configurations
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("roles");

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired() 
                .HasMaxLength(50);

            var roles = Enum
                .GetValues<RoleEnum>()
                .Select(r => new RoleEntity
                {
                    Id = Guid.NewGuid(),
                    Name = r.ToString()
                });

            builder.HasData(roles);
        }
    }
}
