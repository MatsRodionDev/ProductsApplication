namespace UserService.API.Dtos.Requests
{
    public record RegisterUserRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password);
}
