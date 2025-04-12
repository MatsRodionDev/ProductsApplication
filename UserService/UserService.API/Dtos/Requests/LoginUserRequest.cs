namespace UserService.API.Dtos.Requests
{
    public record LoginUserRequest(
        string Email,
        string Password);
}
