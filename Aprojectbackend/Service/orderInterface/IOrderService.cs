using Aprojectbackend.DTO.orderDTO;
using AutoMapper.Execution;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aprojectbackend.Service.orderInterface
{
    public interface IOrderService
    {
        //Task<int> CreateOrderAsync(SubmitOrderDto orderDto);  // 添加 CreateOrderAsync 方法

        Task<List<OrderListDto>> GetOrdersByUserIdAsync(int userId);
        Task<OrderCompleteDto> CreateOrderAsync(int userId, SubmitOrderDto orderDto);  // 確保方法接受這兩個引數
        Task<OrderDetailDto> GetOrderDetailByOrderIdAsync(int orderId);
        Task<MemberDataDto> GetMemberDataAsync(int id);
    }
}
