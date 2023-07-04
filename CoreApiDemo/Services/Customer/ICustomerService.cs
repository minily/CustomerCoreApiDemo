namespace CoreApiDemo.Services
{
    public interface ICustomerService
    {
        public void UpdateScore(long customerId, int score);

        public List<CustomerOutput> GetRanking(int start, int end);

        public List<CustomerOutput> GetRankingNearestList(long customerId, int high, int low);

        public List<CustomerOutput> GetRanking(int top);

        public int GetScore(long customerId);
    }
}
