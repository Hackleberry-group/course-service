using Azure;
using Azure.Data.Tables;
using CourseServiceAPI.Interfaces.Queries;
using System.Collections.Concurrent;
using System.Xml;

namespace CourseServiceAPI.Services.Queries;

public class TableStorageQueryService : ITableStorageQueryService
{
    private readonly TableServiceClient _tableServiceClient;

    public TableStorageQueryService(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
    }

    // Get table client and create table if it does not exist
    public async Task<IEnumerable<T>> GetAllEntitiesAsync<T>(string tableName) where T : class, ITableEntity, new()
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName);

        await tableClient.CreateIfNotExistsAsync();

        var entities = new List<T>();

        await foreach (var entity in tableClient.QueryAsync<T>())
        {
            entities.Add(entity);
        }

        return entities;
    }

    // Get table client and create table if it does not exist, GetEntityAsync
    public async Task<T> GetEntityAsync<T>(string tableName, string partitionKey, string rowKey)
        where T : class, ITableEntity, new()
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName);

        await tableClient.CreateIfNotExistsAsync();

        try
        {
            return await tableClient.GetEntityAsync<T>(partitionKey, rowKey);
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    public Task<IEnumerable<T>> GetEntitiesByPartitionKeyAsync<T>(string tableName, string partitionKey) where T : class, ITableEntity, new()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetEntitiesByRowKeyAsync<T>(string tableName, string rowKey) where T : class, ITableEntity, new()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteEntityAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new()
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName);

        await tableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public Task DeleteEntitiesByPartitionKeyAsync<T>(string tableName, string partitionKey) where T : class, ITableEntity, new()
    {
        throw new NotImplementedException();
    }

    public Task DeleteEntitiesByRowKeyAsync<T>(string tableName, string rowKey) where T : class, ITableEntity, new()
    {
        throw new NotImplementedException();
    }

    public Task DeleteEntitiesByRowKeyAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new()
    {
        throw new NotImplementedException();
    }
}