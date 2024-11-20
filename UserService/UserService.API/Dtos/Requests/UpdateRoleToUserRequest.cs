namespace UserService.API.Dtos.Requests
{
    public record UpdateRoleToUserRequest(
        Guid UserId,
        Guid RoleId);
}
