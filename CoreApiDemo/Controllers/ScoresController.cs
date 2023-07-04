using CoreApiDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly ILogger<ScoresController> _logger;
        private readonly ICustomerService _customerService;

        public ScoresController(ILogger<ScoresController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        /// <summary>
        /// Update Score
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpPost("customer/{customerId}/score/{score}")]
        public IActionResult UpdateScore(long customerId, int score)
        {
            // 参数校验
            if (customerId <= 0)
            {
                return Ok(new
                {
                    status = -1,
                    msg = "参数错误，请输入正确的CustomerId"
                });
            }
            if (score < -1000 || score > 1000)
            {
                return Ok(new
                {
                    status = -1,
                    msg = "参数错误，请输入正确的Score"
                });
            }

            _logger.LogTrace($"ScoresController.UpdateScore2 customerid:{customerId} update score:{score}");

            _customerService.UpdateScore(customerId, score);

            // 因未对data做要求，规范此处可返回自定义的ResponesData结构
            // 包括 status,msg,data 等常用属性
            return Ok(new
            {
                status = 0,
                msg = "成功"
            });
        }

        /// <summary>
        /// Get customers by rank
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet("leaderboard/start/{start}/end/{end}")]
        public ActionResult<IEnumerable<CustomerOutput>> GetRanking(int start, int end)
        {
            return _customerService.GetRanking(start, end);
        }

        /// <summary>
        /// Get customers by customerid
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("leaderboard/{customerId}/high/{high}/low/{low}")]
        public ActionResult<IEnumerable<CustomerOutput>> GetScore(long customerId, int high, int low)
        {
            // 参数校验
            if (customerId <= 0)
            {
                return Ok(new
                {
                    status = -1,
                    msg = "参数错误，请输入正确的CustomerId"
                });
            }

            return _customerService.GetRankingNearestList(customerId, high, low);
        }

        /// <summary>
        /// leaderboard top
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet("leaderboard/top/{top}")]
        public ActionResult<IEnumerable<CustomerOutput>> GetRanking(int top)
        {
            return _customerService.GetRanking(top);
        }

        /// <summary>
        /// Get customer 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("customer/{customerId}")]
        public ActionResult<int> GetScore(long customerId)
        {
            return _customerService.GetScore(customerId);
        }
    }
}