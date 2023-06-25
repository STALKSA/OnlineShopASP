using System.Collections.Concurrent;

namespace OnlineShopASP
{
    public interface ICatalog
    {
        List<Product> GetProducts();
        void AddProduct(Product product);
        void ClearProducts();
        void DeleteProductById(Guid productId);
        Product GetProductById(Guid productId);
        void UpdateProductById(Guid productId, Product newProduct);
    }
}