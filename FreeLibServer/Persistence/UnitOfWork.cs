using System.Threading.Tasks;
using FreeLibServer.Core;

namespace FreeLibServer.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FreeLibDbContext _context;
        public UnitOfWork(FreeLibDbContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}