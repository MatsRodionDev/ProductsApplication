using UserService.BLL.Interfaces.Hashers;

namespace UserService.BLL.Common.Hashers
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password) =>
           BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
