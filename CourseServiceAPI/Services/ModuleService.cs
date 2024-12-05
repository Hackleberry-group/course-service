using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Module;
using CourseServiceAPI.Models.Topic;

namespace CourseServiceAPI.Services
{
    public class ModuleService : IModuleService
    {

        private readonly ITableStorageQueryService _tableStorageQueryService;
        private readonly ITableStorageCommandService _tableStorageCommandService;

        private const string TableName = EntityConstants.ModuleTableName;
        private const string PartitionKey = EntityConstants.ModulePartitionKey;

        public ModuleService(ITableStorageQueryService tableStorageQueryService,
        ITableStorageCommandService tableStorageCommandService)
        {
            _tableStorageQueryService = tableStorageQueryService;
            _tableStorageCommandService = tableStorageCommandService;
        }

        public async Task<IEnumerable<Module>> GetModulesAsync()
        {
            return await _tableStorageQueryService.GetAllEntitiesAsync<Module>(TableName);
        }

        public async Task<Module> GetModuleByIdAsync(Guid id)
        {
            return await _tableStorageQueryService.GetEntityAsync<Module>(TableName, PartitionKey, id.ToString());
        }

        public Task<IEnumerable<Module>> GetModulesByCourseIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Module> CreateModuleAsync(Module module)
        {
            return await _tableStorageCommandService.AddEntityAsync(TableName, module);
        }

        public async Task<Module> PutModuleByIdAsync(Guid id, Module module)
        {
            module.PartitionKey = PartitionKey;
            module.RowKey = id.ToString();

            return await _tableStorageCommandService.UpdateEntityAsync(TableName, module);
        }

        public async Task DeleteModuleAsync(Guid id)
        {
            await _tableStorageQueryService.DeleteEntityAsync(TableName, PartitionKey, id.ToString());
            //TODO: Delete all topics by module id
        }

        
    }
}
