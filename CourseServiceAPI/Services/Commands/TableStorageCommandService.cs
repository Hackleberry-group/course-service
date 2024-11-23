using Azure;
using Azure.Data.Tables;
using CourseServiceAPI.Interfaces.Commands;

namespace CourseServiceAPI.Services.Commands;

public class TableStorageCommandService : ITableStorageCommandService
{
    private readonly TableServiceClient _tableServiceClient;

    public TableStorageCommandService(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
    }

    public async Task AddEntityAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName);

        await tableClient.CreateIfNotExistsAsync();

        try
        {
            await tableClient.AddEntityAsync(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding entity: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateEntityAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName);

        var entityToUpdate = await tableClient.GetEntityAsync<T>(entity.PartitionKey, entity.RowKey);

        if (entityToUpdate != null)
        {
            entityToUpdate.Value.ETag = entity.ETag;

            try
            {
                await tableClient.UpdateEntityAsync(entityToUpdate.Value, entity.ETag, TableUpdateMode.Replace);
            }
            catch (RequestFailedException ex) when (ex.Status  == 412)
            {
                throw new InvalidOperationException("The entity has been modified since it was last retrieved.", ex);
            }
        }
        else
        {
            throw new InvalidOperationException("The entity does not exist.");
        }
    }
}