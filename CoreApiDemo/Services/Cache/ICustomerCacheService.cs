namespace CoreApiDemo.Services
{
    public interface ICustomerCacheService
    {
        /// <summary>
        /// 获取倒序排名列表
        /// </summary>
        /// <returns></returns>
        public List<CustomerOutput> GetRankingList(int top);

        /// <summary>
        /// 根据分数区间获取排行榜列表
        /// </summary>
        /// <returns></returns>
        public List<CustomerOutput> GetRankingList(int start, int end);

        /// <summary>
        /// 根据客户排名获取相对于该客户的高位和地位列表
        /// </summary>
        /// <returns></returns>
        public List<CustomerOutput> GetRankingNearestList(long customerId, int high, int low);

        /// <summary>
        /// 增加缓存（存在则覆盖）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set(long key, int value);

        /// <summary>
        /// 增加指定的整数值，可以是负值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Incrby(long key, int value);

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetValue(long key);
    }
}
