using Azure.Data.Tables;

namespace CourseServiceAPI.Interfaces.Queries;

public interface ITableStorageQueryService
{
    Task<IEnumerable<T>> GetAllEntitiesAsync<T>(string tableName) where T : class, ITableEntity, new();

    Task<T> GetEntityAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new();

    Task<IEnumerable<T>> GetEntitiesByPartitionKeyAsync<T>(string tableName, string partitionKey) where T : class, ITableEntity, new();

    Task<IEnumerable<T>> GetEntitiesByRowKeyAsync<T>(string tableName, string rowKey) where T : class, ITableEntity, new();

    Task DeleteEntityAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new();

    Task DeleteEntitiesByPartitionKeyAsync<T>(string tableName, string partitionKey) where T : class, ITableEntity, new();

    Task DeleteEntitiesByRowKeyAsync<T>(string tableName, string rowKey) where T : class, ITableEntity, new();

    Task DeleteEntitiesByRowKeyAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new();
}