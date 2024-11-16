using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aprojectbackend.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Aprojectbackend.Service.Conmon;
using Aprojectbackend.DTO.productsDTO;

namespace Aprojectbackend.Controllers.productController
{
    [Route("api/[controller]")]
    [ApiController]
    public class productController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactor;
        private readonly AprojectContext _context;

        public productController(AprojectContext context, IHttpClientFactory clientFactor)
        {
            _context = context;
            _clientFactor = clientFactor;
        }

        // GET: api/product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TProduct>>> GetTProducts(
            string keyword = null,
            string category = null,
            decimal? minPrice = null,
            decimal? maxPrice = null)
{
                // 建立基本查詢，僅查詢 TProducts
                IQueryable<TProduct> query = _context.TProducts;

                // 關鍵字篩選
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(p => p.FProdName.Contains(keyword) || p.FProdDescription.Contains(keyword));
                }

                // 類別篩選
                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(p => p.FProdCategory == category);
                }

                // 價格範圍篩選
                if (minPrice.HasValue)
                {
                    query = query.Where(p => p.FProdSellingPrice >= minPrice.Value);
                }
                if (maxPrice.HasValue)
                {
                    query = query.Where(p => p.FProdSellingPrice <= maxPrice.Value);
                }

                // 查詢資料庫並轉換圖片
                List<TProduct> products = await query.ToListAsync();
                foreach (var product in products)
                {
                    product.FProdImage = ConvertImage.ConvertImageBase64(product.FProdImage);
                }

                return products;

                //List<TProduct> products = await _context.TProducts.ToListAsync();
                //foreach (var product in products)
                //{
                //    product.FProdImage = ConvertImage.ConvertImageBase64(product.FProdImage);
                //}
                ////不建議回傳_context.TProducts.ToListAsync()
                //return products;
            }

        // GET: api/product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TProduct>> GetTProduct(int id)
        {
            var tProduct = await _context.TProducts.FindAsync(id);

            if (tProduct == null)
            {
                return NotFound();
            }
            return tProduct;
        }


        private bool TProductExists(int id)
        {
            return _context.TProducts.Any(e => e.FProductId == id);
        }      
    }
}
