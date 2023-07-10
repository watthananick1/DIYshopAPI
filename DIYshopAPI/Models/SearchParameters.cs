using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class SearchParameters
    {
        public string Category { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Duration { get; set; }
        public int? LatestDays { get; set; }
    }
    public class SalesReportParameters
    {
        public ProductType? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Duration { get; set; }
        public int? LatestDays { get; set; }
        public int? Month { get; set; }
    }

    public class SalesReportItem
    {
        public ProductType? Type { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CustomerPurchaseParameters
    {
        public string? CustomerName { get; set; }
        public int? CustomerId { get; set; }
    }

    public class CustomerPurchase
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int TotalPurchaseAmount { get; set; }
        public decimal AmountPaid { get; set; }
    }

    public class ProductParameters
    {
        public int? Id { get; set; }
        public string? N_Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int Stock { get; set; }
        public ProductType? Type { get; set; }
        public string? ImgPoduct { get; set; }
    }


}
