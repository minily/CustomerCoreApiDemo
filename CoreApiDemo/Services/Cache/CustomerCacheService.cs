namespace CoreApiDemo.Services;

/// <summary>
/// 系统缓存服务
/// </summary>
public class CustomerCacheService : ICustomerCacheService
{
    private readonly Dictionary<long, int> _cacheScore;

    public CustomerCacheService()
    {
        _cacheScore = new Dictionary<long, int>();
    }

    /// <summary>
    /// 获取倒序排名列表
    /// </summary>
    /// <returns></returns>
    public List<CustomerOutput> GetRankingList(int top)
    {
        var list = new List<CustomerOutput>();
        var rankScoreList = _cacheScore.Where(x => x.Value > 0).OrderByDescending(x => x.Value).ThenBy(x => x.Key).Select(x => new { Id = x.Key, Score = x.Value }).Take(top).ToList();

        for (int i = 0; i < rankScoreList.Count; i++)
        {
            var rank = rankScoreList[i];
            var data = new CustomerOutput()
            {
                Rank = i + 1,
                CustomerId = rank.Id,
                Score = rank.Score
            };

            list.Add(data);
        }
        return list;
    }

    /// <summary>
    /// 根据分数区间获取排行榜列表
    /// </summary>
    /// <returns></returns>
    public List<CustomerOutput> GetRankingList(int start, int end)
    {
        var list = new List<CustomerOutput>();
        var tmpList = _cacheScore.Where(x => x.Value > 0).OrderByDescending(x => x.Value).ThenBy(x => x.Key).Select((item, i) => new { Id = item.Key, Score = item.Value, rank = i + 1 });

        var rankList = tmpList.Where(x => x.rank >= start && x.rank <= end).ToList();

        for (int i = 0; i < rankList.Count; i++)
        {
            var item = rankList[i];
            var data = new CustomerOutput()
            {
                Rank = item.rank,
                CustomerId = item.Id,
                Score = item.Score
            };

            list.Add(data);
        }
        return list;
    }

    /// <summary>
    /// 根据客户排名获取相对于该客户的高位和地位列表
    /// </summary>
    /// <returns></returns>
    public List<CustomerOutput> GetRankingNearestList(long customerId, int high, int low)
    {
        var list = new List<CustomerOutput>();
        var rankScoreList = _cacheScore.Where(x => x.Value > 0).OrderByDescending(x => x.Value).ThenBy(x => x.Key).Select(x => new { Id = x.Key, Score = x.Value }).ToList();

        // 1. 判断 customerId 是否在排行榜中
        var result = _cacheScore.TryGetValue(customerId, out var rankScore);
        if (!result)
        {
            // 不存在置0
            rankScore = 0;
            rankScoreList.Add(new { Id = customerId, Score = rankScore });
        }

        // 2. 获取 suctomerId 名字和分数
        var ranking = 0;
        foreach (var item in rankScoreList)
        {
            ranking++;
            if (item.Id == customerId)
            {
                break;
            }
        }

        // 3. 取高位数
        if (high > 0)
        {
            var tmpList = rankScoreList.Where(x => x.Score > rankScore);
            var tmpHigh = tmpList.Skip(tmpList.Count() - high).ToList();

            var count = tmpHigh.Count;  // 不能直接用high，否则不够high时排名错乱
            for (int i = 0; i < tmpHigh.Count; i++)
            {
                var rank = tmpHigh[i];
                var data = new CustomerOutput()
                {
                    Rank = ranking - count + i,
                    CustomerId = rank.Id,
                    Score = rank.Score
                };

                list.Add(data);
            }
        }

        // 4. 添加自己
        list.Add(new CustomerOutput()
        {
            Rank = ranking,
            CustomerId = customerId,
            Score = rankScore
        });

        // 5. 取低位数
        if (low > 0)
        {
            var tmpList = rankScoreList.Where(x => x.Score < rankScore);
            var tmpLow = tmpList.Take(low).ToList();

            for (int i = 0; i < tmpLow.Count; i++)
            {
                var rank = tmpLow[i];
                var data = new CustomerOutput()
                {
                    Rank = ranking + (i + 1),
                    CustomerId = rank.Id,
                    Score = rank.Score
                };

                list.Add(data);
            }
        }

        return list;
    }

    /// <summary>
    /// 增加缓存（存在则覆盖）
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public void Set(long key, int value)
    {
        _cacheScore[key] = value;
    }

    /// <summary>
    /// 增加指定的整数值，可以是负值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public void Incrby(long key, int value)
    {
        if (_cacheScore.Keys.Contains(key))
        {
            _cacheScore[key] += value;
        }
        else
        {
            _cacheScore[key] = value;
        }
    }

    /// <summary>
    /// 获取缓存值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetValue(long key)
    {
        if (_cacheScore.Keys.Contains(key))
        {
            return _cacheScore[key];
        }
        else
        {
            return 0;
        }
    }
}