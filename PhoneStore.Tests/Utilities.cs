using PhoneStore.Core.Domain;
using PhoneStore.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Tests
{
    public static class Utilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Products.AddRange(GetSeedingProducts());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationDbContext db)
        {
            db.Products.RemoveRange(db.Products);
            InitializeDbForTests(db);
        }

        public static List<Product> GetSeedingProducts()
        {
            return new List<Product>()
            {
                new Product { Id = 1, Name = "P1", Processor = "i3", OperatingSystem = "Windows", UserId ="115"},
                new Product { Id = 2, Name = "P2", Processor = "i5", OperatingSystem = "Linux"},
                new Product { Id = 3, Name = "P2", Processor = "i3", OperatingSystem = "MacOS"},
                new Product { Id = 4, Name = "P4", Processor = "i7", OperatingSystem = "Windows"},
                new Product { Id = 5, Name = "P5", Processor = "i3", OperatingSystem = "Windows"}
            };
        }
    }
}
