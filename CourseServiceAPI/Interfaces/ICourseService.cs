using CourseServiceAPI.Models.Course;
using CourseServiceAPI.Models.Module;

namespace CourseServiceAPI.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<Course>> GetCoursesAsync();
    Task<Course> GetCourseByIdAsync(Guid id);
    Task<Course> CreateCourseAsync(Course course);
    Task<Course> PutCourseByIdAsync(Guid id, Course course);
    Task DeleteCourseAsync(Guid id);
}