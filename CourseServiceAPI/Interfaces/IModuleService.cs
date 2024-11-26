using CourseServiceAPI.Models.Topic;
using CourseServiceAPI.Models.Module;

namespace CourseServiceAPI.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<Module>> GetModulesAsync();
        Task<Module> GetModuleByIdAsync(Guid id);
        Task<Module> CreateModuleAsync(Module topic);
        Task<Module> PutModuleByIdAsync(Guid id, Topic topic);
        Task DeleteModuleAsync(Guid id);
    }
}
