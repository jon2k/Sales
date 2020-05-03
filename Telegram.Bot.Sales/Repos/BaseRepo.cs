using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.Repos
{
    public class BaseRepo<T> : IDisposable, IRepo<T> where T : EntityBase, new()
    {
        protected readonly DbSet<T> _table;
        private readonly ApplicationContext _db;
        protected ApplicationContext Context => _db;

        public BaseRepo(ApplicationContext context)
        {          
            _db = context;
            _table = _db.Set<T>();
        }

        public void Dispose()
        {
            _db?.Dispose();
        }


        public void Attach(T entity)
        {
            _table.Attach(entity);
        }
        public async virtual Task<int> Add(T entity)
        {
            _table.Add(entity);
            return await SaveChangesAsync();
        }

        public Task<int> AddRange(IList<T> entities)
        {
            _table.AddRange(entities);
            return SaveChangesAsync();
        }

        public Task<int> Change(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            return SaveChangesAsync();
        }

        public Task<int> Delete(int id, byte[] timeStamp)
        {
            _db.Entry(new T() { Id = id, Timestamp = timeStamp }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }


        public Task<int> Delete(T entity)
        {
            _db.Entry(entity).State = EntityState.Deleted;
            return SaveChangesAsync();
        }

        public virtual T GetOne(int? id) => _table.Find(id);

        public virtual List<T> GetAll() => _table.ToList();

        //  public List<T> ExecuteQuery(string sql) => _table.SqlQuery(sql).ToList();

        //  public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects)
        //     => _table.SqlQuery(sql, sqlParametersObjects).ToList();

        internal async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //Thrown when there is a concurrency error
                //for now, just rethrow the exception
                throw ex;
            }
            catch (DbUpdateException ex)
            {
                //Thrown when database update fails
                //Examine the inner exception(s) for additional 
                //details and affected objects
                //for now, just rethrow the exception
                throw ex;
            }
            /* catch (CommitFailedException ex)
             {
                 //handle transaction failures here
                 //for now, just rethrow the exception
                 throw;
             }*/
            catch (Exception ex)
            {
                //some other exception happened and should be handled
                throw ex;
            }
        }
    }
}
