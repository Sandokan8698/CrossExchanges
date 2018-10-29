using System.Threading.Tasks;

namespace XOProject
{
    public interface IShareRepository : IGenericRepository<Share>
    {
        Task<Share> FindLastBySymbolAsync(string symbol);
    }
}