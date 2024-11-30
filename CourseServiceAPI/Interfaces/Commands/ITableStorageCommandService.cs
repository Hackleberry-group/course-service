using Azure.Data.Tables;

namespace CourseServiceAPI.Interfaces.Commands;

public interface ITableStorageCommandService
{
    Task<T> AddEntityAsync<T>(string tableName, T entity) where T : class, ITableEntity, new();

    Task<T> UpdateEntityAsync<T>(string tableName, T entity) where T : class, ITableEntity, new();
}