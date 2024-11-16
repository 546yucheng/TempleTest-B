// Services/Interfaces/ICartService.cs
using Aprojectbackend.DTO.orderDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aprojectbackend.Service.orderInterface
{
    public interface ICartService
    {
        // 取得指定會員的購物車資料
        Task<List<OCartItemDto>> GetCartItemsByUserIdAsync(int? userId);
    }
}
