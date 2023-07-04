namespace CoreApiDemo.Services
{
    /// <summary>
    /// 客户排行榜返回结果
    /// </summary>
    public class CustomerOutput
    {
        /// <summary>
        /// 客户Id
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }
    }
}
