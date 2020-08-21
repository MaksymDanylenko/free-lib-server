using System.Threading.Tasks;

namespace FreeLibServer.Persistence
{
    public interface IUnitOfWork {
         Task CompleteAsync();
    }
}