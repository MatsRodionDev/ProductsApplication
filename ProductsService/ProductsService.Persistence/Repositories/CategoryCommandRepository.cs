using ProductsService.Domain.Interfaces;
using ProductsService.Domain.Models;

namespace ProductsService.Persistence.Repositories
{
    internal sealed class CategoryCommandRepository(ApplicationDBContext context) : GenericRepository<Category>(context), ICategoryCommandRepository
    {

    }
}
