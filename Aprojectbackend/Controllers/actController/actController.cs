using Aprojectbackend.DTO.actDTO;
using Aprojectbackend.Models;
using ECPay.Payment.Integration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aprojectbackend.Controllers.actController
{
    //[Route("api/[controller]")]
    [Route("act")]
    [EnableCors("All")]
    [ApiController]
    public class actController : ControllerBase
    {
        private readonly AprojectContext _context;
        public IWebHostEnvironment _enviro;

        //建構函式，DI注入
        public actController(AprojectContext context, IWebHostEnvironment p)
        {
            _enviro = p;
            _context = context;
        }

        /// <summary>
        /// 活動首頁，傳篩選的活動資料
        /// </summary>
        /// <returns></returns>
        //GET: act/index
        [HttpGet("index")]
        public async Task<IActionResult> index([FromQuery] actSearch a)
        {
            IQueryable<TActInformation> tempContext = _context.TActInformations;


            //找還沒有整個結束的活動，排除未成立、已結束的活動
            tempContext = tempContext.Where(n => _context.TActDetails.Where(x => x.FStatusId == 5)
                                                                     .Where(x => x.FActId == n.FActId)
                                                                     .Any());

            //活動種類篩選
            if (a.FActCategoryId != 0)
            {
                tempContext = tempContext.Where(n => n.FActCategoryId == a.FActCategoryId);
            }

            IQueryable<actList> result = tempContext
                   .Include(n => n.FActCategory)
                   .Include(n => n.TActImgData)
                   .Select(n => new actList
                   {
                       FActId = n.FActId,
                       FActName = n.FActName,
                       FActLocation = n.FActLocation,
                       FRegFee = n.FRegFee,
                       FActCategory = n.FActCategory.FActCategory,
                       FActImgName = n.TActImgData.FirstOrDefault().FActImgName
                   });

            //關鍵字搜尋
            if (!string.IsNullOrEmpty(a.txtKeyword))
            {
                result = result.Where(n => n.FActName.Contains(a.txtKeyword) ||
                                      n.FActLocation.Contains(a.txtKeyword));
            }

            // 如果沒有找到任何資料
            if (!result.Any())
            {
                return NotFound();
            }

            //故意將回傳值複製一倍，換頁明顯
            //result = result.Concat(result);

            //分頁邏輯處理
            actPagesList actPagesList = new actPagesList();
            actPagesList.pages = (int)Math.Ceiling((double)result.Count() / 8);
            if (actPagesList.pages < a.targetPage)
            {
                a.targetPage = 1;
            }
            actPagesList.actList = result.Skip((a.targetPage - 1) * 8).Take(8).ToList();

            return new JsonResult(actPagesList);
        }

        /// <summary>
        /// 首頁活動
        /// </summary>
        /// <returns></returns>
        [HttpGet("indexAct")]
        public async Task<IActionResult> indexAct()
        {
            IQueryable<TActInformation> tempContext = _context.TActInformations;

            //找出活動狀態"活動正常"(statusid==5)，並取得不重複、最近期的前4個活動id
            List<int?> oederbyActId = _context.TActDetails.Where(x => x.FStatusId == 5)
                                                .OrderBy(x => x.FStartDate)
                                                .Select(x => x.FActId).ToList();
            List<int> listActId = new List<int>();
            foreach (var id in oederbyActId)
            {
                if (!listActId.Contains((int)id) && listActId.Count < 4) listActId.Add((int)id);
            }

            IQueryable<actList> result = tempContext
                   .Where(n => listActId.Contains(n.FActId))
                   .Include(n => n.FActCategory)
                   .Include(n => n.TActImgData)
                   .Select(n => new actList
                   {
                       FActId = n.FActId,
                       FActName = n.FActName,
                       FActLocation = n.FActLocation,
                       FRegFee = n.FRegFee,
                       FActCategory = n.FActCategory.FActCategory,
                       FActImgName = n.TActImgData.FirstOrDefault().FActImgName
                   });

            return new JsonResult(result.ToList().Take(4));
        }

        /// <summary>
        ///活動說明頁 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}/{fuserId?}")]
        public async Task<IActionResult> detail(string id, string? fuserId)
        {
            int actId = Convert.ToInt32(id);
            IQueryable<actDetail> result = _context.TActInformations
                .Where(n => n.FActId == actId)
                   .Include(n => n.FActCategory)
                   .Include(n => n.TActImgData)
                   .Include(n => n.TActDetails)
                   .Select(n => new actDetail
                   {
                       FActId = actId,
                       FActCategory = n.FActCategory.FActCategory,
                       FActName = n.FActName,
                       FActDisplayId = n.FActDisplayId,
                       FActLocation = n.FActLocation,
                       FActDescription = n.FActDescription,
                       FRegFee = n.FRegFee,
                       FMaxNumber = n.FMaxNumber,
                       actDetailDate = n.TActDetails.Select(t => new actDetailDate
                       {
                           FActDetailId = t.FActDetailId,
                           FDisplayId = t.FDisplayId,
                           FStatusId = t.FStatusId,
                           dates = (t.FStartDate == t.FEndDate) ? t.FStartDate.Replace("-", "/") : $"{t.FStartDate.Replace("-", "/")}~{t.FEndDate.Replace("-", "/")}",
                       }).ToList(),
                       FActImgName = n.TActImgData.FirstOrDefault() != null ?
                                   n.TActImgData.FirstOrDefault().FActImgName : "noImage.jpg",
                   });

            var n = result.ToList();
            if (!result.Any())
            {
                return NotFound(); // 如果沒有找到任何資料
            }
            return new JsonResult(result.ToList());
        }


        /// <summary>
        ///活動報名資訊，抓現有紀錄
        /// </summary>
        /// <param name="actDetailId"></param>
        /// <returns></returns>
        [HttpGet("RegRecord/{actDetailId}/{fuserId?}")]
        public async Task<IActionResult> RegRecord(string actDetailId, string? fuserId)
        {
            int IntActDetailId = Convert.ToInt32(actDetailId);
            int actId = (int)_context.TActDetails.Where(n => n.FActDetailId == IntActDetailId).FirstOrDefault().FActId;
            IQueryable<actDetail> result = _context.TActInformations
                .Where(n => n.FActId == actId)
                   .Include(n => n.FActCategory)
                   .Include(n => n.TActImgData)
                   .Include(n => n.TActDetails)
                   .Select(n => new actDetail
                   {
                       FActId = actId,
                       FActCategory = n.FActCategory.FActCategory,
                       FActName = n.FActName,
                       FActDisplayId = n.FActDisplayId,
                       FActLocation = n.FActLocation,
                       FActDescription = n.FActDescription,
                       FRegFee = n.FRegFee,
                       FMaxNumber = n.FMaxNumber,
                       actDetailDate = n.TActDetails.Select(t => new actDetailDate
                       {
                           FActDetailId = t.FActDetailId,
                           FDisplayId = t.FDisplayId,
                           FStatusId = t.FStatusId,
                           dates = (t.FStartDate == t.FEndDate) ? t.FStartDate.Replace("-", "/") : $"{t.FStartDate.Replace("-", "/")}~{t.FEndDate.Replace("-", "/")}",
                       }).ToList(),
                       FActImgName = n.TActImgData.FirstOrDefault() != null ?
                                   n.TActImgData.FirstOrDefault().FActImgName : "noImage.jpg",

                       //actDetail, actReg同一api，姓名、信箱給actReg用
                       //FUserName = !string.IsNullOrEmpty(fuserId) ? _context.TUsers.Where(n => n.FUserId == int.Parse(fuserId))
                       //                                                             .First().FUserName : null,
                       //FUserEmail = !string.IsNullOrEmpty(fuserId) ? _context.TUsers.Where(n => n.FUserId == int.Parse(fuserId))
                       //                                                             .First().FUserEmail : null,
                   });

            var n = result.ToList();
            if (!result.Any())
            {
                return NotFound(); // 如果沒有找到任何資料
            }
            var regRecord = _context.TActEventRegs.Where(n => n.FUserId == Convert.ToInt32(fuserId) && n.FActDetailId == Convert.ToInt32(actDetailId)).FirstOrDefault();

            List<RegDetail> regDetail = new List<RegDetail>();

            if (regRecord == null)
            {
                TUser user = _context.TUsers.Where(n => n.FUserId == Convert.ToInt32(fuserId)).FirstOrDefault();
                regDetail.Add(new RegDetail
                {
                    fRegName = user.FUserName,
                    fRegTel = user.FUserPhone,
                    fRegEmail = user.FUserEmail,
                });
            }
            else
            {
                regDetail = _context.TRegDetails.Where(n => n.FRegId == regRecord.FRegId)
                                                                .Select(n => new RegDetail
                                                                {
                                                                    fRegName = n.FRegName,
                                                                    fRegTel = n.FRegTel,
                                                                    fRegEmail = n.FRegEmail,
                                                                    fRecipientAddress = n.FRecipientAddress,
                                                                }).ToList();
            }

            ActToReg actToReg = new ActToReg
            {
                actDetail = result.ToList(),
                RegDetail = regDetail,
            };

            return Ok(actToReg);
        }



        /// <summary>
        /// 儲存活動報名資料進DB
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [HttpPost("actReg")]
        public async Task<IActionResult> actReg([FromBody] RegRequest e)
        {
            if (e == null)
            {
                return BadRequest("none");
            }
            int actFee = (int)(from actInfo in _context.TActInformations
                               join actdetail in _context.TActDetails
                               on actInfo.FActId equals actdetail.FActId
                               where actdetail.FActDetailId == e.FActDetailId
                               select actInfo).FirstOrDefault().FRegFee;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                TActEventReg newTEventReg = new TActEventReg
                {
                    FStatusId = 7,
                    FUserId = e.FUserId,
                    FActDetailId = e.FActDetailId,
                    FActPayment = e.TotalAmount,
                    FRegDate = DateTime.Now.ToString("yyyy/MM/dd"),
                };
                await _context.TActEventRegs.AddAsync(newTEventReg);
                await _context.SaveChangesAsync();

                foreach (var item in e.regDetail)
                {
                    TRegDetail newTRegDetail = new TRegDetail
                    {
                        FRegId = newTEventReg.FRegId,
                        FRegName = item.fRegName,
                        FRegTel = item.fRegTel,
                        FRegEmail = item.fRegEmail,
                        FRecipientAddress = item.fRecipientAddress,
                    };
                    await _context.TRegDetails.AddAsync(newTRegDetail);
                }
                await _context.SaveChangesAsync();
                if (actFee == 0)
                {
                    newTEventReg.FStatusId = 6;
                    newTEventReg.FDispRegNum = "actReg" + newTEventReg.FRegId.ToString();
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Ok(new { status = "complete", data = newTEventReg.FRegId });
                }
                await transaction.CommitAsync();
                return Ok(new { status = "success", data = newTEventReg.FRegId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                //throw ex;
                Console.WriteLine(ex.Message);
                return BadRequest(new { status = "error", message = "儲存異常，請稍後再試" });
            }
        }
    }
}

