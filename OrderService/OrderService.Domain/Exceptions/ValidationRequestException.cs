using System.Text.Json;

namespace OrderService.Domain.Exceptions
{
    public class ValidationRequestException(string message) : Exception(message);
}
