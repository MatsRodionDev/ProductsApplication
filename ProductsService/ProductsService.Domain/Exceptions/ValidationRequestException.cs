using System.Text.Json;

namespace ProductsService.Domain.Exceptions
{
    public class ValidationRequestException(string[] errors) : Exception(JsonSerializer.Serialize(errors))
    {
    }
}
