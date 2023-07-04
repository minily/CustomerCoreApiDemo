namespace CoreApiDemo.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerCacheService _sysCacheService;

        public CustomerService(ICustomerCacheService sysCacheService)
        {
            _sysCacheService = sysCacheService;
        }

        public void UpdateScore(long customerId, int score)
        {
            _sysCacheService.Incrby(customerId, score);
        }

        public List<CustomerOutput> GetRanking(int start, int end)
        {
            return _sysCacheService.GetRankingList(start, end);
        }

        public List<CustomerOutput> GetRankingNearestList(long customerId, int high, int low)
        {
            return _sysCacheService.GetRankingNearestList(customerId, high, low);
        }

        public int GetScore(long customerId)
        {
            return _sysCacheService.GetValue(customerId);
        }

        public List<CustomerOutput> GetRanking(int top)
        {
            return _sysCacheService.GetRankingList(top);
        }
    }
}
