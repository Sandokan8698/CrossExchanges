using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace XOProject
{
    public class ShareRepository : GenericRepository<Share>, IShareRepository
    {
        public ShareRepository(ExchangeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Share> FindLastBySymbolAsync(string symbol)
        {
            return _dbContext.Set<Share>().Where(x => x.Symbol.Equals(symbol)).LastOrDefaultAsync();
        }
    }
}