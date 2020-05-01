using System.Collections.Generic;
using System.Threading.Tasks;

namespace Telegram.Bot.Sales.Repos
{
    public interface IRepo<T>
    {
        Task<int> Add(T entity);
        Task<int> AddRange(IList<T> entities);
        Task<int> Change(T entity);
        Task<int> Delete(int id, byte[] timeStamp);
        Task<int> Delete(T entity);
        T GetOne(int? id);
        List<T> GetAll();

        //  List<T> ExecuteQuery(string sql);
        //  List<T> ExecuteQuery(string sql, object[] sqlParametersObjects);
    }
}
