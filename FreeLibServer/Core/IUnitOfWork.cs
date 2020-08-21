using System.Threading.Tasks;

namespace FreeLibServer.Core
{
    public interface IUnitOfWork {
         Task CompleteAsync();
    }
}