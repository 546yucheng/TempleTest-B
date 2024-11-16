using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aprojectbackend.Models;
using Microsoft.AspNetCore.Cors;
using Aprojectbackend.DTO.orderDTO;

namespace Aprojectbackend.Controllers.orderController
{
    [Route("api/[controller]")]
    [EnableCors("All")]
    [ApiController]
    public class DonateController : ControllerBase
    {
        private readonly AprojectContext _context;

        public DonateController(AprojectContext context)
        {
            _context = context;
        }
        
        // GET: api/Donate
        /// <summary>
        /// 取得捐款紀錄
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDonations(int page = 1, int pageSize = 5, int userId = 124)
        {
            // 假設你有一個資料庫查詢
            var donations = _context.TDonations
                                    .Where(d => d.FUserId == userId) // 篩選會員 ID 為 124 的捐款紀錄
                                    .Skip((page - 1) * pageSize)    // 分頁
                                    .Take(pageSize)                 // 分頁
                                    .ToList();

            var totalDonations = _context.TDonations.Count(d => d.FUserId == userId); // 計算該會員的總捐款數量
            var totalPages = (int)Math.Ceiling(totalDonations / (double)pageSize); // 計算總頁數

            return Ok(new
            {
                donations = donations,
                totalPages = totalPages
            });
        }

        // return Ok(new XXDTO
        //    {
        //        donations = donations,
        //        totalPages = totalPages
        //    });
        //上面那段要再寫一段以下的DTO包起來才對(而且DTO要放在DTO.cs檔裡
        // public class XXDTO()
        // {
        //    public IList<DonationDTO> Donations { get; set; }
        //    public int TotalPages { get; set; }
        // }



        // POST: api/Donate
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// 新增一筆捐款
        /// </summary>
        /// <param name="tDonation"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TDonation>> PostTDonation(TDonation tDonation)
        {
            // 假設 124 是測試用的固定會員 ID
            tDonation.FUserId = 124;

            // 新增至資料庫
            _context.TDonations.Add(tDonation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostTDonation), new { id = tDonation.FDonationId }, tDonation);
        }
    }
}
