namespace UserService.API.Dtos.Requests
{
    public record PaginationRequest(
        int Page = 1,
        int PageSize = 5);
}
