using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Module;
using CourseServiceAPI.Models.Topic;
using HackleberrySharedModels.Exceptions;

namespace CourseServiceAPI.Services
{
    public class ModuleService : IModuleService
    {

        private readonly ITableStorageQueryService _tableStorageQueryService;
        private readonly ITableStorageCommandService _tableStorageCommandService;
        private readonly ITopicService _topicService;

        private const string TableName = EntityConstants.ModuleTableName;
        private const string PartitionKey = EntityConstants.ModulePartitionKey;

        public ModuleService(ITableStorageQueryService tableStorageQueryService,
        ITableStorageCommandService tableStorageCommandService,
        ITopicService exerciseService)
        {
            _tableStorageQueryService = tableStorageQueryService;
            _tableStorageCommandService = tableStorageCommandService;
            _topicService = exerciseService;
        }

        public async Task<IEnumerable<Module>> GetModulesAsync()
        {
            return await _tableStorageQueryService.GetAllEntitiesAsync<Module>(TableName);
        }

        public async Task<Module> GetModuleByIdAsync(Guid id)
        {
            return await _tableStorageQueryService.GetEntityAsync<Module>(TableName, PartitionKey, id.ToString());
        }

        public async Task<IEnumerable<Module>> GetModulesByCourseIdAsync(Guid id)
        {
            var modules = await _tableStorageQueryService.GetEntitiesByFilterAsync<Module>(TableName, id.ToFilter<Module>("CourseId"));
            
            foreach (var module in modules) {
                var filter = Guid.Parse(module.RowKey).ToFilter<Topic>("ModuleId");
                var topics = await _tableStorageQueryService.GetEntitiesByFilterAsync<Topic>(EntityConstants.TopicTableName, filter);
                module.TopicIds = topics.Select(t => Guid.Parse(t.RowKey)).ToList();
            }
            return modules;
        }

        public async Task<Module> CreateModuleAsync(Module module)
        {
            module.PartitionKey = PartitionKey;
            module.RowKey = Guid.NewGuid().ToString();
            await _tableStorageCommandService.AddEntityAsync(TableName, module);
            return module;
        }

        public async Task<Module> PutModuleByIdAsync(Guid id, Module module)
        {
            module.PartitionKey = PartitionKey;
            module.RowKey = id.ToString();
            await _tableStorageCommandService.UpdateEntityAsync(TableName, module);
            return module;
        }

        public async Task DeleteModuleAsync(Guid id)
        {
            var module = await _tableStorageQueryService.GetEntityAsync<Module>(TableName, PartitionKey, id.ToString());
            if (module == null)
            {
                throw new NotFoundException();
            }
            
            var filter = id.ToFilter<Topic>("ModuleId");
            var topics = await _tableStorageQueryService.GetEntitiesByFilterAsync<Topic>(EntityConstants.TopicTableName, filter);
            
            foreach (var topic in topics)
            {
                await _topicService.DeleteTopicAsync(Guid.Parse(topic.RowKey));
            }

            await _tableStorageQueryService.DeleteEntityAsync(TableName, PartitionKey, id.ToString());
        }
    }
}
