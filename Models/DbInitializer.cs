namespace TestApp.Models
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;
            }

            var products = new Product[]
            {
                new Product { Name = "Product 1", Price = 10.0m },
                new Product { Name = "Product 2", Price = 20.0m },
                new Product { Name = "Product 3", Price = 30.0m }
            };

            foreach (var p in products)
            {
                context.Products.Add(p);
            }

            context.SaveChanges();
        }
    }
}