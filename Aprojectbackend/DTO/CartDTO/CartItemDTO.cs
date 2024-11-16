using System.Reflection.Metadata.Ecma335;
using System.Security.Authentication.ExtendedProtection;

namespace Aprojectbackend.DTO.CartDTO
{
    public class CartItemDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public string FProductName { get; set; } = default!; // 保證新增物件時會給他值
        public int FCartItemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FProdSellingPrice { get; set; }

        public decimal FCartPrice { get; set; }
        public int? FItemQuantity { get; set; }
        public string FProdDescription { get; set; }
        public string FProdImage { get; set; }

    
    }
}
