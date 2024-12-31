using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.DAL.Entities;

namespace UserService.DAL.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(e => e.FirstName)
               .HasColumnName("first_name") 
               .IsRequired() 
               .HasMaxLength(50); 

            builder.Property(e => e.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            builder.Property(e => e.IsActivated)
               .HasColumnName("is_activated")
               .IsRequired();

            builder.Property(e => e.RoleId)
                .HasColumnName("role_id")
                .IsRequired();

            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
        }
    }
}
