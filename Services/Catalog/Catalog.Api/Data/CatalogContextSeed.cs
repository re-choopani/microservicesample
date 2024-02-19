using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertMany(GetSeedData());
            }
        }
        private static IEnumerable<Product> GetSeedData()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "Name",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile  = "",
                    Price = 554,
                    Category = "Category",
                }
            };
        }
    }
}
