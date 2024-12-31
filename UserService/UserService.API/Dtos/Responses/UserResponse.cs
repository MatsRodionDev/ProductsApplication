namespace UserService.API.Dtos.Responses
{
    public record UserResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email);
}
