using Aprojectbackend.DTO.orderDTO;
using Aprojectbackend.Models;
using Aprojectbackend.Service.orderInterface;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper.Execution;
using static Aprojectbackend.DTO.linepayDTO.linepayDTO;

namespace Aprojectbackend.Service.orderService
{
    public class OrderService : IOrderService
    {
        private readonly AprojectContext _context;

        public OrderService(AprojectContext context)
        {
            _context = context;
        }

        //建立訂單
        //public async Task<OrderCompleteDto> CreateOrderAsync(int userId, SubmitOrderDto orderDto)
        //{
        //    var newOrder = new TOrder
        //    {
        //        FUserId = userId,
        //        FOrderDate = DateTime.Now.ToString("yyyyMMdd"), // 日期格式轉為字串
        //        FRecepientName = orderDto.RecepientName,
        //        FRecepientAddress = orderDto.RecepientAddress,
        //        FRecepientPhone = orderDto.RecepientPhone,
        //        FRecepientEmail = orderDto.RecepientEmail,
        //        FShippingInfo = orderDto.ShippingInfo,
        //        FPaymentInfo = orderDto.PaymentInfo,
        //        FOrderRemarks = orderDto.OrderRemarks,
        //        //FTotalPrice = orderDto.CartItems.Sum(item => item.Price),  計算總金額
        //        FTotalPrice = orderDto.CartItems.Sum(item => item.Price*item.Quantity),

        //        // 設置訂單、付款和配送的預設狀態
        //        FOrderStatus = "待處理",
        //        FPaymentStatus = false, // 未付款
        //        FShippingStatus = false // 未配送
        //    };

        //    // 新增訂單並儲存至資料庫
        //    _context.TOrders.Add(newOrder);
        //    await _context.SaveChangesAsync();

        //    // 取得新訂單的 ID
        //    int orderId = newOrder.FOrderId;

        //    // 建立訂單明細並新增到資料庫
        //    foreach (var cartItem in orderDto.CartItems)
        //    {
        //        // 將 CartItemDto 轉換為 CartItemDataDto
        //        var cartItemData = new CartItemDataDto
        //        {
        //            ProductId = cartItem.ProductId,
        //            Quantity = cartItem.Quantity,
        //            Price = cartItem.Price*cartItem.Quantity // 使用 CartItemDto 的小計yu
        //        };

        //        var orderDetail = new TOrderDetail
        //        {
        //            FOrderId = orderId,
        //            FProductId = cartItemData.ProductId,
        //            FQuantity = cartItemData.Quantity,
        //            FPrice = cartItemData.Price // 儲存到資料庫
        //        };

        //        _context.TOrderDetails.Add(orderDetail);
        //    }

        //    // 提交訂單明細的變更
        //    await _context.SaveChangesAsync();

        //    //return orderId;

        //    //回傳 OrderCompleteDto(訂單編號)
        //    return new OrderCompleteDto
        //    {
        //        OrderId = newOrder.FOrderId
        //    };
        //}
        public async Task<OrderCompleteDto> CreateOrderAsync(int userId, SubmitOrderDto orderDto)
        {
            // 計算訂單總金額
            decimal totalAmount = orderDto.CartItems.Sum(item => item.Price * item.Quantity);

            // 創建訂單
            var newOrder = new TOrder
            {
                FUserId = userId,
                FOrderDate = DateTime.Now.ToString("yyyyMMdd"),
                FRecepientName = orderDto.RecepientName,
                FRecepientAddress = orderDto.RecepientAddress,
                FRecepientPhone = orderDto.RecepientPhone,
                FRecepientEmail = orderDto.RecepientEmail,
                FShippingInfo = orderDto.ShippingInfo,
                FPaymentInfo = orderDto.PaymentInfo,
                FOrderRemarks = orderDto.OrderRemarks,
                FTotalPrice = totalAmount,
                FOrderStatus = "待處理",
                FPaymentStatus = false,
                FShippingStatus = false
            };

            // 新增訂單並儲存至資料庫
            _context.TOrders.Add(newOrder);
            await _context.SaveChangesAsync();

            // 建立訂單明細並新增到資料庫
            foreach (var cartItem in orderDto.CartItems)
            {
                var orderDetail = new TOrderDetail
                {
                    FOrderId = newOrder.FOrderId,
                    FProductId = cartItem.ProductId,
                    FQuantity = cartItem.Quantity,
                    FPrice = cartItem.Price
                };

                _context.TOrderDetails.Add(orderDetail);
            }

            // 保存訂單明細到資料庫
            await _context.SaveChangesAsync();

            // 準備回應資料
            var response = new OrderCompleteDto
            {
                OrderId = newOrder.FOrderId,
                Amount = totalAmount,
                Currency = "TWD" // 假設為台幣
            };

            return response;
        }


        //訂單列表
        public async Task<List<OrderListDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await (
                from o in _context.TOrders
                where o.FUserId == userId
                select new OrderListDto
                {
                    OrderId = o.FOrderId,
                    OrderDate = string.IsNullOrEmpty(o.FOrderDate) ? "" : o.FOrderDate,
                    TotalPrice = o.FTotalPrice ?? 0,  // 若 TotalAmount 為 null，則預設為 0
                    PaymentStatus = (o.FPaymentStatus.HasValue ? o.FPaymentStatus.Value.ToString() : "未付款"),
                    ShippingStatus = (o.FShippingStatus.HasValue ? o.FShippingStatus.Value.ToString() : "未配送"),
                    OrderStatus = string.IsNullOrEmpty(o.FOrderStatus) ? "待處理" : o.FOrderStatus
                }).ToListAsync();

            return orders;
        }

        //訂單明細
        public async Task<OrderDetailDto> GetOrderDetailByOrderIdAsync(int orderId)
        {
            var orderDetail = await (from order in _context.TOrders
                                     where order.FOrderId == orderId
                                     select new OrderDetailDto
                                     {
                                         UserId = order.FUserId ?? 0,
                                         OrderId = order.FOrderId,
                                         OrderDate = order.FOrderDate ?? "N/A", //
                                         TotalPrice = order.FTotalPrice ?? 0,
                                         RecepientName = order.FRecepientName,
                                         RecepientAddress = order.FRecepientAddress,
                                         RecepientPhone = order.FRecepientPhone,
                                         RecepientEmail = order.FRecepientEmail,
                                         ShippingInfo = order.FShippingInfo,
                                         PaymentInfo = order.FPaymentInfo,
                                         OrderRemarks = order.FOrderRemarks,
                                         Products = (from orderDetail in _context.TOrderDetails
                                                     join prod in _context.TProducts
                                                     on orderDetail.FProductId equals prod.FProductId
                                                     where orderDetail.FOrderId == order.FOrderId
                                                     select new OCartItemDto
                                                     {
                                                         ProductId = prod.FProductId,
                                                         ProdName = prod.FProdName,
                                                         ProdSellingPrice = prod.FProdSellingPrice ?? 0,
                                                         Quantity = orderDetail.FQuantity ?? 0
                                                     }).ToList()
                                     }).FirstOrDefaultAsync();

            return orderDetail;
        }

        //代入會員資料
        public async Task<MemberDataDto> GetMemberDataAsync(int id)
        {
            var user = await _context.TUsers.FindAsync(id);
            if (user == null) return null;

            // 將 TUser 表轉換為 MemberDataDto
            var memberDto = new MemberDataDto
            {
                Name = user.FUserName,
                Email = user.FUserEmail,
                Phone = user.FUserPhone,
                Address = user.FUserAddress
            };

            return memberDto;
        }

    }
}
