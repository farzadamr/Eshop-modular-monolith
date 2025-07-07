namespace Catalog.Data.Seed;

public static class InitialData
{
    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(new Guid("855C6202-D975-4EF4-B8E8-0E548AF9E649"), "IPhone 16 Pro Max", ["Category1"], "Long Description", "://file1", 13000),
            Product.Create(new Guid("DDE80815-1899-4F4D-8A87-2B12964A9A5E"), "IPhone 15 Pro Max", ["Category1"], "Long Description", "://file2", 12000),
            Product.Create(new Guid("560DFEB6-B7C8-4FDC-A618-4E0624A06AAE"), "IPhone 14 Pro Max", ["Category2"], "Long Description", "://file3", 11000),
            Product.Create(new Guid("BC45DF48-E591-4A53-B4CD-772BDF468002"), "IPhone 13 Pro Max", ["Category2"], "Long Description", "://file4", 10000),
        };
}
