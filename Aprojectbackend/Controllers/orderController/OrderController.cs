using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aprojectbackend.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Security.Claims;
using Aprojectbackend.DTO.orderDTO;
using Aprojectbackend.Service.orderInterface;
using static Aprojectbackend.DTO.linepayDTO.linepayDTO;
using Aprojectbackend.Service.Linepayinterface;
using System.Diagnostics;
using Aprojectbackend.Service.orderService;

namespace Aprojectbackend.Controllers.orderController
{
    [Route("api/[controller]")]
    [EnableCors("All")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AprojectContext _context;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IOrderEmailService _orderEmailService;
        private readonly ILinepayService _linepayService;

        // 將 IOrderService 和 ICartService 以參數的形式傳入構造函數
        public OrderController(AprojectContext context, IOrderService orderService, ICartService cartService, ILinepayService linepayService, IOrderEmailService orderEmailService)
        {
            _context = context;
            _orderService = orderService;
            _cartService = cartService;
            _orderEmailService = orderEmailService;
            _linepayService = linepayService;
        }

        // GET: api/Order
        /// <summary>
        /// 取得購物車資料
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrderPage(int userId)
        {
            // 取得指定會員的購物車資料
            var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);

            if (cartItems == null || !cartItems.Any())
            {
                return NotFound("No items found in the cart.");
            }

            // 計算總金額
            var totalPrice = cartItems.Sum(item => item.Price);

            var orderPageDto = new OrderPageDto
            {
                CartItems = cartItems,
                TotalPrice = totalPrice,
            };

            return Ok(orderPageDto);
        }

        // GET: api/Order/Delete
        /// <summary>
        /// 清空購物車
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteCartItemsByUserId(int userId)
        {
            // 1.撈出使用者的購物車資料
            List<TCart> carts = _context.TCarts.Where(e => e.FUserId == userId).ToList();

            // 2.每筆都設定刪除
            _context.TCarts.RemoveRange(carts);

            // 3.儲存
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Order/SubmitOrder
        /// <summary>
        /// 新增一筆訂單並寄送完成訂單的郵件
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        [HttpPost("SubmitOrder")]
        public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderDto orderDto)
        {
            // 從目前登入的使用者身分取得 UserId
            //var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (userIdString == null)
            //    return Unauthorized();
            Console.WriteLine(1234);
            int userId = orderDto.UserId;

            // 調用 OrderService 來創建訂單
            //int orderId = await _orderService.CreateOrderAsync(userId, orderDto);
            OrderCompleteDto orderCompleteDto = await _orderService.CreateOrderAsync(userId, orderDto);

            // 調用 LinePayService 來創建交易
            var cartItems = orderDto.CartItems;
            var packages = new List<PackageDto>();
            var products = new List<LinepayProductDto>();
            foreach (var cartItem in cartItems)
            {
                products.Add(new LinepayProductDto
                { 
                    Name = cartItem.ProductName,
                    Price = (int)cartItem.Price,
                    Quantity = cartItem.Quantity,
                });
            }
            packages.Add(new PackageDto
            {
                Id = Guid.NewGuid().ToString(),
                Amount = products.Sum(item => item.Price*item.Quantity),
                Products = products,
            });
            PaymentRequestDto paymentRequestDto = new PaymentRequestDto { 
                Amount= packages.Sum(item => item.Amount),
                Currency= "TWD",
                OrderId= orderCompleteDto.OrderId.ToString(),
                RedirectUrls= new RedirectUrlsDto {
                    ConfirmUrl = "http://localhost:4200/orderconfirm",
                    CancelUrl = "https://localhost:4200/order/linepay-callback"
                },
                Packages= packages
            };

            Console.WriteLine("參數:"+ paymentRequestDto);
            PaymentResponseDto paymentResponseDto = await _linepayService.SendPaymentRequest(paymentRequestDto);

            return Ok(paymentResponseDto);

            // 確保訂單成功建立
            //if (orderCompleteDto == null)
            //{
            //    return BadRequest("訂單建立失敗");
            //}

            //await _orderEmailService.SendOrderCompletionEmailAsync(orderCompleteDto.OrderId.ToString());

            // 回傳訂單完成的 DTO，包含訂單編號
            return Ok(orderCompleteDto);
        }

        // GET: api/Order/OrderList/{userId}
        /// <summary>
        /// 訂單列表（分頁）
        /// </summary>
        /// <param name="userId">會員 ID</param>
        /// <param name="pageNumber">頁碼（從 1 開始）</param>
        /// <param name="pageSize">每頁顯示的項目數</param>
        /// <returns></returns>
        [HttpGet("OrderList/{userId}")]
        public async Task<IActionResult> GetOrderList(int userId, int pageNumber = 1, int pageSize = 10)
        {
            // 從 Service 取得指定會員的訂單列表
            var orderListDto = await _orderService.GetOrdersByUserIdAsync(userId);

            if (orderListDto == null || !orderListDto.Any())
            {
                return NotFound("No orders found.");
            }

            // 計算分頁資訊
            var totalOrders = orderListDto.Count();
            var totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);

            // 使用 Skip 和 Take 進行分頁
            var pagedOrders = orderListDto
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(order => new OrderListDto
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    TotalPrice = order.TotalPrice,
                    PaymentStatus = order.PaymentStatus,
                    ShippingStatus = order.ShippingStatus,
                    OrderStatus = order.OrderStatus,
                    TotalPages = totalPages,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                })
                .ToList();

            return Ok(pagedOrders);
        }


        // GET: api/Order/OrderDetail/{orderId}
        /// <summary>
        /// 取得訂單明細
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("OrderDetail/{orderId}")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            var orderDetailDto = await _orderService.GetOrderDetailByOrderIdAsync(orderId);

            if (orderDetailDto == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(orderDetailDto);
        }

        // GET: api/Order/member/{id}
        /// <summary>
        /// 代入會員資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("member/{id}")]
        public async Task<IActionResult> GetMemberData(int id)
        {
            var memberData = await _orderService.GetMemberDataAsync(id);
            if (memberData == null)
            {
                return NotFound();
            }
            return Ok(memberData);
        }
    }
}
