using Aprojectbackend.DTO.orderDTO;
using Aprojectbackend.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Aprojectbackend.Service.orderInterface;


namespace Aprojectbackend.Service.orderService
{
    public class CartService : ICartService
    {
        private readonly AprojectContext _context;

        public CartService(AprojectContext context)
        {
            _context = context;
        }

        // 取得指定會員的購物車資料
        public async Task<List<OCartItemDto>> GetCartItemsByUserIdAsync(int? userId)
        {
            //return await _context.TCarts  // 使用 TCarts 而不是 CartItems
            //    .Where(c => c.FUserId == userId)
            //    .Select(c => new CartItemDto
            //    {
            //    ProductId =(int) c.FProductId,
            //    //ProdName = c.Product.Name,   假設你有對應的 Product 物件
            //    ProdSellingPrice = c.FProductId.FProdSellingPrice,  // 同上
            //    Quantity = c.FItemQuantity ?? 0,  // 預防 null
            //    Price = c.FProduct.FProdSellingPrice * (c.FItemQuantity ?? 0)
            //    })
            //    .ToListAsync();
            return await (from cart in _context.TCarts
                          join product in _context.TProducts
                          on cart.FProductId equals product.FProductId
                          where cart.FUserId == userId
                          select new OCartItemDto
                          {
                              ProductId = cart.FProductId ?? 0,
                              ProdName = product.FProdName,  // 從 TProduct 取得名稱
                              ProdSellingPrice = product.FProdSellingPrice ?? 0,  // 從 TProduct 取得價格
                              Quantity = cart.FItemQuantity ?? 0,
                              //Price = (product.FProdSellingPrice ?? 0) * (cart.FItemQuantity ?? 0)  // 計算小計
                          }).ToListAsync();
        }
    }
}
