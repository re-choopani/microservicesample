using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(string id);
        Task<IEnumerable<Product>> GetProductByNameAsync(string productName);
        Task<IEnumerable<Product>> GetProductByCategoryAsync(string category);
        Task CreateProduct(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string id);
    }
}
