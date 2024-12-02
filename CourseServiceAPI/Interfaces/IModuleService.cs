using CourseServiceAPI.Models.Topic;
using CourseServiceAPI.Models.Module;

namespace CourseServiceAPI.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<Module>> GetModulesAsync();
        Task<Module> GetModuleByIdAsync(Guid id);
        Task<IEnumerable<Module>> GetModulesByCourseIdAsync(Guid id);
        Task<Module> CreateModuleAsync(Module module);
        Task<Module> PutModuleByIdAsync(Guid id, Module module);
        Task DeleteModuleAsync(Guid id);
    }
}
