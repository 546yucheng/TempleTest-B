namespace Aprojectbackend.DTO.ProductDTO
{
    public class ProductDTO
    {
        public int FProductId { get; set; }
        public string FProdName { get; set; }
        public decimal FProdPrice { get; set; }
        public decimal FProdSellingPrice { get; set; }
        public string FProdImage { get; set; }
        public string FProdCategory { get; set; }
        public string FProdPromoStartDate { get; set; }
        public string FProdPromoEndDate { get; set; }
    }
}
