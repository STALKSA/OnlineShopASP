using System.Collections.Concurrent;

namespace OnlineShopASP
{
    public interface ICatalog
    {
        void AddProduct(Product product);
        void ClearProducts();
        void DeleteProductById(Guid productId);
        Product GetProductById(Guid productId);
        ConcurrentDictionary<Guid, Product> GetProducts();
        void UpdateProductById(Guid productId, Product newProduct);
    }
}