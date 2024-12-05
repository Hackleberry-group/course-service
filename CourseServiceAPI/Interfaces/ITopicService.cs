using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Topic;

namespace CourseServiceAPI.Interfaces
{
    public interface ITopicService
    {
        Task<IEnumerable<Topic>> GetTopicsAsync();
        Task<Topic> CreateTopicAsync(Topic topic);
        Task<Topic> GetTopicByIdAsync(Guid id);
        Task<Topic> PutTopicByIdAsync(Guid id, Topic topic);
        Task DeleteTopicAsync(Guid id);
        Task<IEnumerable<Topic>> GetTopicsByModuleIdAsync(Guid moduleId);
    }
}