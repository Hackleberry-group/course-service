using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Course;
using CourseServiceAPI.Models.Module;

namespace CourseServiceAPI.Services
{
    public class CourseService : ICourseService
    {
        private readonly ITableStorageQueryService _tableStorageQueryService;
        private readonly ITableStorageCommandService _tableStorageCommandService;
        private readonly IModuleService _moduleService;

        private const string TableName = EntityConstants.CourseTableName;
        private const string PartitionKey = EntityConstants.CoursePartitionKey;

        public CourseService(ITableStorageQueryService tableStorageQueryService,
            ITableStorageCommandService tableStorageCommandService,
            IModuleService moduleService)
        {
            _tableStorageQueryService = tableStorageQueryService;
            _tableStorageCommandService = tableStorageCommandService;
            _moduleService = moduleService;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            var courses = await _tableStorageQueryService.GetAllEntitiesAsync<Course>(TableName);

            foreach (var course in courses) {
                var filter = Guid.Parse(course.RowKey).ToFilter<Module>("CourseId");
                var modules = await _tableStorageQueryService.GetEntitiesByFilterAsync<Module>(EntityConstants.ModuleTableName, filter);
                course.ModuleIds = modules.Select(m => Guid.Parse(m.RowKey)).ToList();
            }
            return courses;
        }

        public async Task<Course> GetCourseByIdAsync(Guid id)
        {
            return await _tableStorageQueryService.GetEntityAsync<Course>(TableName, PartitionKey, id.ToString());
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            course.RowKey = Guid.NewGuid().ToString();
            course.PartitionKey = PartitionKey;
            await _tableStorageCommandService.AddEntityAsync(TableName, course);
            return course;
        }

        public async Task<Course> PutCourseByIdAsync(Guid id, Course course)
        {
            course.PartitionKey = PartitionKey;
            course.RowKey = id.ToString();
            await _tableStorageCommandService.UpdateEntityAsync(TableName, course);
            return course;
        }

        public async Task DeleteCourseAsync(Guid id)
        {
            await _tableStorageQueryService.DeleteEntityAsync(TableName, PartitionKey, id.ToString());
            // TODO: Call user service to delete the group-course link
            // TODO: Call module service to remove the modules with course id
        }
    }
}

