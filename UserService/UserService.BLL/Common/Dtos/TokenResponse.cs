namespace UserService.BLL.Common.Responses
{
    public record TokenResponse(
        string AccessToken,
        string RefreshToken);
}
