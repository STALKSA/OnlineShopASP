using System.Collections.Concurrent;

namespace OnlineShopASP
{
    public class InMemoryCatalog : ICatalog
    {
        private ConcurrentDictionary<Guid, Product> _products = GenerateProducts(10);


        public ConcurrentDictionary<Guid, Product> GetProducts()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                foreach (var product in _products.Values)
                {
                    product.Price *= 0.7m; // Снижаем цену на 30%
                }
            }

            return _products;
        }


        public Product GetProductById(Guid productId)
        {
            if (!_products.TryGetValue(productId, out var product))
            {
                throw new KeyNotFoundException($"Id {productId} не найден.");
            }
            else
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    product.Price *= 0.7m; // Снижаем цену на 30%
                }
                return product;
            }
        }


        public void AddProduct(Product product)
        {
            if (!_products.TryAdd(product.Id, product))
            {
                throw new ArgumentException("Товар уже существует.", nameof(product));
            };

        }


        public void DeleteProductById(Guid productId)
        {
            if (!_products.TryRemove(productId, out _))
            {
                throw new InvalidOperationException("Товар не найден.");
            }

        }


        public void ClearProducts()
        {
            _products.Clear();

        }


        public void UpdateProductById(Guid productId, Product newProduct)
        {
            if (!_products.TryGetValue(productId, out var oldProductValue))
            {
                throw new KeyNotFoundException($" ID {newProduct.Id} не найден.");
            }


        }


        private static ConcurrentDictionary<Guid, Product> GenerateProducts(int count)
        {
            var random = new Random();
            var products = new ConcurrentDictionary<Guid, Product>();

            var productNames = new string[]
            {
            "Cracking the Coding Interview",
            "Code Complete",
            "Clean Code",
            "Refactoring",
            "Head First Design Patterns",
            "Patterns of Enterprise Application Architecture",
            "Working Effectively with Legacy Code",
            "The Clean Coder",
            "Introduction to Algorithms",
            "The Pragmatic Programmer"
            };



            for (int i = 0; i < count; i++)
            {
                var name = productNames[i];
                var price = random.Next(50, 2000);
                var producedAt = DateTime.Now.AddDays(-random.Next(1, 30));
                var expiredAt = producedAt.AddDays(random.Next(1, 365));
                var stock = random.NextDouble() * 100;

                var product = new Product(name, price);
                product.Description = "Описание" + name;
                product.ProducedAt = producedAt;
                product.ExpiredAt = expiredAt;
                product.Stock = stock;
                products.TryAdd(product.Id, product);
            }

            return products;
        }

        internal Task GetProductsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
