using System.Text.Json;

namespace ProductsService.Domain.Exceptions
{
    public class ValidationRequestException(string message) : Exception(message)
    {
    }
}
