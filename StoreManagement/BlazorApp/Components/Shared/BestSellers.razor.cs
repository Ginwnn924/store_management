namespace BlazorApp.Components.Shared;

public partial class BestSellers
{
    private List<BestSellerItem> BestSellerProducts = new()
    {
        new() { Id = 1, Name = "Rau cải xanh hữu cơ", Category = "Rau củ", Price = 25000, OriginalPrice = 35000, ImageUrl = "https://images.unsplash.com/photo-1540420773420-3366772f4999?w=400", SoldCount = 2350, Discount = 29 },
        new() { Id = 2, Name = "Thịt ba chỉ heo tươi", Category = "Thịt", Price = 125000, OriginalPrice = 145000, ImageUrl = "https://images.unsplash.com/photo-1602470520998-f4a52199a3d6?w=400", SoldCount = 1890, Discount = 14 },
        new() { Id = 3, Name = "Trứng gà ta (10 quả)", Category = "Trứng", Price = 45000, OriginalPrice = 50000, ImageUrl = "https://images.unsplash.com/photo-1582722872445-44dc5f7e3c8f?w=400", SoldCount = 3420, Discount = 10 },
        new() { Id = 4, Name = "Sữa tươi Vinamilk 1L", Category = "Sữa", Price = 32000, OriginalPrice = 36000, ImageUrl = "https://images.unsplash.com/photo-1563636619-e9143da7973b?w=400", SoldCount = 4560, Discount = 11 },
        new() { Id = 5, Name = "Táo Fuji nhập khẩu", Category = "Trái cây", Price = 89000, OriginalPrice = 110000, ImageUrl = "https://images.unsplash.com/photo-1560806887-1e4cd0b6cbd6?w=400", SoldCount = 1230, Discount = 19 },
        new() { Id = 6, Name = "Tôm sú tươi sống", Category = "Hải sản", Price = 195000, OriginalPrice = 220000, ImageUrl = "https://images.unsplash.com/photo-1565680018434-b513d5e5fd47?w=400", SoldCount = 890, Discount = 11 },
        new() { Id = 7, Name = "Dầu ăn Neptune 1L", Category = "Đồ khô", Price = 52000, OriginalPrice = 58000, ImageUrl = "https://images.unsplash.com/photo-1474979266404-7eaacbcd87c5?w=400", SoldCount = 2100, Discount = 10 },
    };

    private string FormatPrice(decimal price)
    {
        return price.ToString("N0") + "đ";
    }

    private class BestSellerItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string ImageUrl { get; set; } = "";
        public int SoldCount { get; set; }
        public int Discount { get; set; }
    }
}

