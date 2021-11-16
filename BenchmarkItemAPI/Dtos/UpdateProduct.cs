namespace BenchmarkItemAPI.Dtos
{
    public class UpdateProduct
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public bool InStock { get; set; }
        public int CategoryId { get; set; }
    }
}
