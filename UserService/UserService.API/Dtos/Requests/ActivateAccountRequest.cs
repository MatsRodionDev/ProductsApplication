namespace UserService.API.Dtos.Requests
{
    public record ActivateAccountRequest(
        Guid UserId,
        int ActivateCode);
}
