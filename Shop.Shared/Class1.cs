namespace Shop.Shared
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public string passwordhash { get; set; } = string.Empty;
        public string role { get; set; } = "User";
    }

    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; } = 0;
        public string category { get; set; }
        public string? image_url { get; set; }
        public string game { get; set; }
        public string game_key { get; set; }
        public string? created_by { get; set; }
        public DateTime created_at { get; set; }
    }
    public class Order
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; } = string.Empty;
        public string product_game { get; set; } = string.Empty;
        public string product_category { get; set; } = string.Empty;
        public string buyer_name { get; set; } = string.Empty;
        public string seller_name { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int quantity { get; set; } = 1;
        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }


    public class PagedResponse<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<T> Products { get; set; } = new List<T>();
    }
}
