using Google.Protobuf.WellKnownTypes;

namespace ITI.GRPCLAB.Client.Models
{
    public static class ProductList
    {

        public static List<Product> Products = new List<Product>() {
             new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 1500,
                Quantity = 10,
                Category = 1,
                ExpireDate = Timestamp.FromDateTime(DateTime.UtcNow.AddYears(3))  // Example: 3 years from now
            },
            new Product
            {
                Id = 2,
                Name = "Sofa",
                Price = 800,
                Quantity = 5,
                Category = 0,
                ExpireDate = Timestamp.FromDateTime(DateTime.UtcNow.AddYears(5))  // Example: 5 years from now
            },
            new Product
            {
                Id = 3,
                Name = "Apple",
                Price = 1,
                Quantity = 100,
                Category = 2,
                ExpireDate = Timestamp.FromDateTime(DateTime.UtcNow.AddMonths(1))  // Example: 1 month from now
            }
        };
    }
}
