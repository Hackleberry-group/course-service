using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models;

namespace CourseServiceAPI.Services;

public class CourseService : ICourseService
{
    public IEnumerable<Course> GetCourses()
    {
        return new List<Course>
        {
            new Course { Id = 1, Title = "Programming 2" },
            new Course { Id = 2, Title = "Java" },
            new Course { Id = 3, Title = "Php ..." }
        };
    }

    public Course CreateCourse(Course course)
    {
        // Logic to create a course can be added here
        return course;
    }
}