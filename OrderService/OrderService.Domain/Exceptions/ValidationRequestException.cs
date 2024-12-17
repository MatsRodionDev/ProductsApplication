using System.Text.Json;

namespace OrderService.Domain.Exceptions
{
    public class ValidationRequestException(string[] errors) : Exception(JsonSerializer.Serialize(errors));
}
