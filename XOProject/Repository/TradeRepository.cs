using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;

namespace XOProject
{
    public class TradeRepository : GenericRepository<Trade>, ITradeRepository
    {
        public TradeRepository(ExchangeContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task<List<TradeAnalysis>> GetAnalysis(string symbol)
        {
           return (from trade in _dbContext.Set<Trade>()
                where trade.Symbol == symbol
                group trade by trade.Action
                into grp
                select new TradeAnalysis
                {
                    Maximum = grp.Max(t => t.NoOfShares),
                    Minimum = grp.Min(t => t.NoOfShares),
                    Average = (decimal)grp.Average(t => t.NoOfShares),
                    Sum = grp.Sum(t => t.NoOfShares),
                    Action = grp.Key
                }).ToListAsync();
        }
    }
}