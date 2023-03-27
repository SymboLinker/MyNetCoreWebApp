using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FunDb;

public interface IFunDb
{
    Task<int> InsertAsync<T>(T record, CancellationToken cancellationToken = default) where T : notnull;
    Task<IEnumerable<T>> QueryAsync<T>(Func<IEnumerable<T>, IEnumerable<T>>? query = null) where T : notnull;
    Task<T> QuerySingleAsync<T>(Func<T, bool> predicate) where T : notnull;
    Task<T?> QuerySingleOrDefaultAsync<T>(Func<T, bool> predicate) where T : notnull;
    Task<int> DeleteAsync<T>(Func<T, bool> predicate, CancellationToken cancellationToken = default)  where T : notnull;
}

public static class __FunDbInternals
{
    public static void AddFunDb(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IFunDb>(new Db());
    }

    public class Db : IFunDb
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<object, byte?>> _dict = new();
        private SemaphoreSlim _slim = new(1);
        private Dictionary<string, int> InsertCountPerTable = new();

    public async Task<int> InsertAsync<T>(T record, CancellationToken cancellationToken = default) where T : notnull
        {
            ConcurrentDictionary<object, byte?> table = GetTable<T>();
            await _slim.WaitAsync(cancellationToken);
            try
            {
                if (!table.TryAdd(record, null))
                {
                    throw new Exception($"The instance cannot be inserted because it already exists.");
                }
                var tableName = GetTableName<T>();
                if (!InsertCountPerTable.TryAdd(tableName, 1))
                {
                    InsertCountPerTable[tableName] = InsertCountPerTable[tableName] + 1;
                }
                var id = InsertCountPerTable[tableName];
                try
                {
                    ((dynamic)record).Id = id;
                }
                catch { }
                return id;
            }
            finally
            {
                _slim.Release();
            }
        }

        public async Task<T> QuerySingleAsync<T>(Func<T, bool> predicate) where T : notnull
        {
            return (await QuerySingleOrDefaultAsync(predicate)) ?? throw new Exception($"Could not find a {typeof(T).Name} that matches predicate {predicate}.");
        }

        public async Task<T?>QuerySingleOrDefaultAsync<T>(Func<T, bool> predicate) where T : notnull
        {
            return (await QueryAsync<T>(records => records.Where(predicate))).SingleOrDefault();
        }

        public async Task<int> DeleteAsync<T>(Func<T, bool> predicate, CancellationToken cancellationToken) where T : notnull
        {
            var table = GetTable<T>();
            await _slim.WaitAsync(cancellationToken);
            try
            {
                var toDelete = (await QueryAsync<T>(records => records.Where(predicate))).ToArray();
                foreach (var record in toDelete)
                {
                    table.Remove(record, out _);
                }
                return toDelete.Length;
            }
            finally
            {
                _slim.Release();
            }
        }

        public Task<IEnumerable<T>> QueryAsync<T>(Func<IEnumerable<T>, IEnumerable<T>>? query = null) where T : notnull
        {
            var records = GetTable<T>().Keys.Select(x => (T)x);
            if (query is null)
            {
                return Task.FromResult(records);
            }
            return Task.FromResult(query(records).AsEnumerable());
        }

        private ConcurrentDictionary<object, byte?> GetTable<T>() where T : notnull
        {
            var tableName = GetTableName<T>();
            return _dict.GetOrAdd(tableName, new ConcurrentDictionary<object, byte?>());
        }

        private static string GetTableName<T>()
        {
            return typeof(T).Name;
        }
    }
}
